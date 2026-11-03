#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainLand_DungeonEditorWindow : EditorWindow
    {
        public static TerrainManager terrainManager;
        public static Obstacle obstacle;
        public DungeonGenerator data = new DungeonGenerator();
        public const string NAME = "DungeonGenerator data";
        public static void OpenWindow(TerrainManager TerrainManagerObject, Obstacle lead)
        {
            terrainManager = TerrainManagerObject;
            obstacle = lead;
            GetWindow<TerrainLand_DungeonEditorWindow>(NAME);
        }

        #region Functions
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
        #endregion
        #region OnGUI
        private void OnGUI()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.ChangeCheck_Begin();
            data.Width = System.Math.Max(1, EditorGUILayout.IntField("Width", data.Width));
            data.Height = System.Math.Max(1, EditorGUILayout.IntField("Height", data.Height));
            if (BasicEditorTools.ChangeCheck_End())
            {
                Repaint();
            }
            data.minRoomSize = BasicEditorTools.IntField("Min room size", data.minRoomSize);
            data.maxRoomSize = BasicEditorTools.IntField("Max room size", data.maxRoomSize);
            data.maxRooms = BasicEditorTools.IntField("Max rooms", data.maxRooms);
            BasicEditorTools.Box_Close();

            //
            BasicEditorTools.Buttons(new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Close",()=>{ this.Close(); }),
                new BasicEditorTools.StringActionStruct("Generate",generate)
            }, 2);
        }
        private void generate()
        {
            if (terrainManager == null)
            {
                onClose();
                return;
            }
            data.GenerateMap();
            Undo.SetCurrentGroupName(NAME);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(terrainManager.gameObject, NAME);
            Undo.RecordObject(terrainManager, NAME);
            var array = terrainManager.DungeonGenerateAlgorithm(data, obstacle);
            Undo.RecordObject(terrainManager.terrainData, NAME);
            foreach (var node in array)
            {
                if (node == null) continue;
                Undo.RegisterCreatedObjectUndo(node, NAME);
            }
            Undo.CollapseUndoOperations(group);
            onClose();
        }
        private void onClose()
        {
            terrainManager = null;
            this.Close();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope(ObstacleEditorWindow.highlightColor))
            {
                if (terrainManager == null) return;
                Vector3 center = terrainManager.transform.position;
                if (obstacle) center = obstacle.Position;
                float x = data.Width;
                float y = data.Height;
                Handles.DrawWireCube(center, new Vector3(x, 1.0f, y));
            }
        }
        #endregion
    }
}
#endif