#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{

    public class TerrainLand_StairwayEditorWindow : EditorWindow
    {
        public const string NAME = "Stairway";
        public static TerrainManager terrainManager;
        public static Obstacle obstacle;
        public TerrainManager.StairwayStruct data = new TerrainManager.StairwayStruct()
        {
            lenght = 2,
            groundConnection = false,
            face = CubeFace.Forward,
        };
        //
        public static void OpenWindow(TerrainManager terrainManagerObject, Obstacle lead)
        {
            terrainManager = terrainManagerObject;
            obstacle = lead;
            GetWindow<TerrainLand_StairwayEditorWindow>(NAME);
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
        #region onGUI
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            BasicEditorTools.ChangeCheck_Begin();
            data.lenght = System.Math.Max(1, EditorGUILayout.IntField("Lenght", data.lenght));
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicDrawEditorTools.Repaint();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            BasicEditorTools.ChangeCheck_Begin();
            data.face = (CubeFace)BasicEditorTools.DropDownList("Direction", (int)data.face, System.Enum.GetNames(typeof(CubeFace)));
            if (!data.face.CanCreateSuggestionCubeFrame()) data.face = CubeFace.Forward;
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicDrawEditorTools.Repaint();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            BasicEditorTools.ChangeCheck_Begin();
            data.groundConnection = EditorGUILayout.Toggle("Ground connection", data.groundConnection);
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicDrawEditorTools.Repaint();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            data.autoIntegration = EditorGUILayout.Toggle("Auto integration", data.autoIntegration);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

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
            Undo.SetCurrentGroupName(NAME);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(terrainManager.gameObject, NAME);
            Undo.RecordObject(terrainManager, NAME);

            var array = terrainManager.StairwayAlgorithm(data, obstacle);
            Undo.RecordObject(terrainManager.terrainData, NAME);
            foreach (var node in array)
            {
                if (node == null) continue;
                Undo.RegisterCreatedObjectUndo(node, NAME);
            }
            Undo.CollapseUndoOperations(group);
            onClose();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope())
            {
                drawStairway();
            }
        }
        private void drawStairway()
        {
            if (terrainManager == null) return;
            Vector3 startPosition = terrainManager.transform.position;
            if (obstacle) startPosition = obstacle.Position;
            int j = 0;
            for (int i = 0; i <= data.lenght; i++)
            {
                Vector3 position = startPosition + Obstacle.getDirection(data.face) * 2 * Obstacle.SIZE * i;
                if (data.groundConnection)
                {
                    for (int k = 0; k < j; k++)
                    {
                        Vector3 p = position + Obstacle.getDirection(CubeFace.Up) * 2 * Obstacle.SIZE * k;
                        Handles.DrawWireCube(p, 2 * Obstacle.SIZE * Vector3.one);
                    }
                }
                position += Obstacle.getDirection(CubeFace.Up) * 2 * Obstacle.SIZE * j++;
                Handles.DrawWireCube(position, 2 * Obstacle.SIZE * Vector3.one);
            }
        }

        #endregion
        private void onClose()
        {
            terrainManager = null;
            obstacle = null;
            this.Close();
        }
    }
}
#endif