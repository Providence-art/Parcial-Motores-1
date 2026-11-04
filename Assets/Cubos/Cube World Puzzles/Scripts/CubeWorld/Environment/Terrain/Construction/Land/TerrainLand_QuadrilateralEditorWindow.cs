#if UNITY_EDITOR
using System.Collections.Generic;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainLand_QuadrilateralEditorWindow : EditorWindow
    {
        public static GameObject terrainManagerObject;
        public static List<GameObject> obstacles;
        public int width = 10;
        public int length = 10;
        public int maxHeight = 3;
        public float heightRate = .1f;
        public float constructionFactor = .9f;
        public bool autoIntegration = false;
        public static void OpenWindow(GameObject TerrainManagerObject, List<GameObject> obstaclesList = null)
        {
            terrainManagerObject = TerrainManagerObject;
            obstacles = obstaclesList;
            GetWindow<TerrainLand_QuadrilateralEditorWindow>("Quadrilateral data");
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
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            width = System.Math.Max(1, EditorGUILayout.IntField("Width", width));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            length = System.Math.Max(1, EditorGUILayout.IntField("Length", length));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            maxHeight = System.Math.Max(1, EditorGUILayout.IntField("Max height", maxHeight));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            heightRate = BasicEditorTools.FloatField("Height rate", heightRate);
            if (heightRate > TerrainLand_Quadrilateral.MAX_C) heightRate = TerrainLand_Quadrilateral.MAX_C;
            else if (heightRate < TerrainLand_Quadrilateral.MIN_C) heightRate = TerrainLand_Quadrilateral.MIN_C;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            constructionFactor = BasicEditorTools.FloatField("Construction factor", constructionFactor);
            if (constructionFactor > TerrainLand_Quadrilateral.MAX_C) constructionFactor = TerrainLand_Quadrilateral.MAX_C;
            else if (constructionFactor < TerrainLand_Quadrilateral.MIN_C) constructionFactor = TerrainLand_Quadrilateral.MIN_C;
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            autoIntegration = BasicEditorTools.Toggle("Auto integration", autoIntegration);
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
            TerrainManager terrainManager; ;
            if (terrainManagerObject == null || (terrainManager = terrainManagerObject.GetComponent<TerrainManager>()) == null)
            {
                onClose();
                return;
            }
            TerrainLand_Quadrilateral result = new TerrainLand_Quadrilateral();
            result.width = width;
            result.length = length;
            result.heightRate = heightRate;
            result.maxHeight = maxHeight;
            result.setConstructionFactor(constructionFactor);
            if (obstacles == null || obstacles.Count == 0) { simpleGenerate(result, terrainManager); }
            else
            {
                foreach (var obstacle in obstacles)
                {
                    if (obstacle == null) continue;
                    Obstacle func = obstacle.GetComponent<Obstacle>();
                    if (func == null) continue;
                    simpleGenerate(result, terrainManager, func);
                }
            }
            onClose();
        }
        private void simpleGenerate(TerrainLand_Quadrilateral result, TerrainManager terrainManager, Obstacle obstacle = null)
        {
            if (!result.Create()) return;
            Undo.SetCurrentGroupName("Generate terrain - Quadrilateral");
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(terrainManager.gameObject, "Generate terrain - Quadrilateral");
            Undo.RecordObject(terrainManager, "Generate terrain - Quadrilateral");

            var array = terrainManager.TerrainLand_GenerateQuadrilateral(result, obstacle, autoIntegration);
            Undo.RecordObject(terrainManager.terrainData, "Generate terrain - Quadrilateral");
            foreach (var node in array)
            {
                if (node == null) continue;
                Undo.RegisterCreatedObjectUndo(node, "Generate terrain - Quadrilateral");
            }
            Undo.CollapseUndoOperations(group);
        }
        private void onClose()
        {
            terrainManagerObject = null;
            this.Close();
        }
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope(ObstacleEditorWindow.highlightColor))
            {
                if (terrainManagerObject == null) return;
                Vector3 center = terrainManagerObject.transform.position;

                float x = width;
                float y = length;
                Handles.DrawWireCube(center, new Vector3(x, 1.0f, y));
            }
        }
        #endregion
    }
}
#endif