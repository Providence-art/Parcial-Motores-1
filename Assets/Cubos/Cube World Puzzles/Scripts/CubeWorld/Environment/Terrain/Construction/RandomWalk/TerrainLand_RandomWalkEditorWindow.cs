#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainLand_RandomWalkEditorWindow : EditorWindow
    {
        public static TerrainManager terrainManager;
        public static Obstacle obstacle;
        public TerrainManager.RandomWalkStruct randomWalkStruct = new TerrainManager.RandomWalkStruct()
        {
            height = 50,
            width = 50,
            steps = 1000
        };
        public const string NAME = "RandomWalk data";
        public static void OpenWindow(TerrainManager TerrainManagerObject, Obstacle lead)
        {
            terrainManager = TerrainManagerObject;
            obstacle = lead;
            GetWindow<TerrainLand_RandomWalkEditorWindow>(NAME);
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
            randomWalkStruct.width = System.Math.Max(1, EditorGUILayout.IntField("Count", randomWalkStruct.width));
            randomWalkStruct.height = System.Math.Max(1, EditorGUILayout.IntField("Count", randomWalkStruct.height));
            if (BasicEditorTools.ChangeCheck_End())
            {
                Repaint();
            }
            randomWalkStruct.steps = System.Math.Max(1, EditorGUILayout.IntField("Count", randomWalkStruct.steps));
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
            if ( terrainManager == null)
            {
                onClose();
                return;
            }
            Undo.SetCurrentGroupName(NAME);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(terrainManager.gameObject, NAME);
            Undo.RecordObject(terrainManager, NAME);
            var array = terrainManager.GenerateRandomWalk(randomWalkStruct, obstacle);
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
                float x = randomWalkStruct.width;
                float y = randomWalkStruct.height;
                Handles.DrawWireCube(center, new Vector3(x, 1.0f, y));
            }
        }
        #endregion
    }
}
#endif