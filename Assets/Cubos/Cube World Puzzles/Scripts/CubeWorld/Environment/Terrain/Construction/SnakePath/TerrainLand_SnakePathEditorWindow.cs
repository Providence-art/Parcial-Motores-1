#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainLand_SnakePathEditorWindow : EditorWindow
    {
        public static TerrainManager terrainManager;
        public static Obstacle obstacle;
        public TerrainManager.SnakeLineData snakeLineData;
        public const string NAME = "SnakeLine data";
        public static void OpenWindow(TerrainManager TerrainManagerObject, Obstacle lead)
        {
            terrainManager = TerrainManagerObject;
            obstacle = lead;
            GetWindow<TerrainLand_SnakePathEditorWindow>(NAME);
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            snakeLineData.count = System.Math.Max(1, EditorGUILayout.IntField("Count", snakeLineData.count));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            snakeLineData.heightRate = BasicEditorTools.FloatField("Height rate", snakeLineData.heightRate);
            if (snakeLineData.heightRate > TerrainManager.SnakeLineData.MAX_Coefficient) snakeLineData.heightRate = TerrainManager.SnakeLineData.MAX_Coefficient;
            else if (snakeLineData.heightRate < TerrainManager.SnakeLineData.MIN_Coefficient) snakeLineData.heightRate = TerrainManager.SnakeLineData.MIN_Coefficient;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            //
            BasicEditorTools.Buttons(new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Close",()=>{ this.Close(); }),
                new BasicEditorTools.StringActionStruct("Generate",generate)
            }, 2);
        }
        private void generate()
        {
            if (obstacle == null || terrainManager == null)
            {
                onClose();
                return;
            }
            Undo.SetCurrentGroupName(NAME);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(terrainManager.gameObject, NAME);
            Undo.RecordObject(terrainManager, NAME);
            var array = terrainManager.GenerateSnakeLine(obstacle, snakeLineData);
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
    }
}
#endif

