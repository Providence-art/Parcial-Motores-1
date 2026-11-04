#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainLand_DrawLandEditorWindow : EditorWindow
    {
        public static GameObject terrainManagerObject;
        public int width = 2;
        public int lenght = 2;
        public int maxHeight = 1;
        public float heightRate = 0.0f;
        public float constructionFactor = 1f;
        public bool autoIntegration = false;
        public Color ObstacleColor = Color.white;
        public Color NoObstacleColor = Color.yellow;
        //
        protected bool[] map;
        private int x, y;
        private Vector2 scrollPosition;
        public static void OpenWindow(GameObject TerrainManagerObject)
        {
            terrainManagerObject = TerrainManagerObject;
            GetWindow<TerrainLand_DrawLandEditorWindow>("Draw land");
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
            width = System.Math.Max(1, EditorGUILayout.IntField("Width", width));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            lenght = System.Math.Max(1, EditorGUILayout.IntField("Lenght", lenght));
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
            autoIntegration = EditorGUILayout.Toggle("Auto integration", autoIntegration);
            //
            //constructionFactor = BasicEditorTools.FloatField("Construction factor", constructionFactor);
            //if (constructionFactor > TerrainLand_Quadrilateral.MAX_C) constructionFactor = TerrainLand_Quadrilateral.MAX_C;
            //else if (constructionFactor < TerrainLand_Quadrilateral.MIN_C) constructionFactor = TerrainLand_Quadrilateral.MIN_C;
            //
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            ObstacleColor = EditorGUILayout.ColorField("Has Obstacle", ObstacleColor);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            NoObstacleColor = EditorGUILayout.ColorField("Has No Obstacle", NoObstacleColor);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //
            if (x != width || y != lenght)
            {
                x = width;
                y = lenght;
                map = new bool[x * y];
                BasicDrawEditorTools.Repaint();
            }
            //
            Color defaultColor = GUI.color;
            BasicEditorTools.Scroll_Begin(ref scrollPosition);
            BasicEditorTools.Box_Open();
            for (int j = 0; j < lenght; j++)
            {
                if (j == 0)
                {
                    GUILayout.BeginHorizontal();
                    //BasicEditorTools.Label(string.Empty);
                    for (int k = 0; k < width; k++)
                    {
                        GUILayout.BeginVertical();
                        BasicEditorTools.Label((k + 1).ToString());
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                for (int i = 0; i < width; i++)
                {
                    //if (i == 0)
                    //{
                    //    GUILayout.BeginVertical();
                    //    BasicEditorTools.Label((j + 1).ToString());
                    //    GUILayout.EndVertical();
                    //}
                    int index = i * y + j;
                    GUILayout.BeginVertical();
                    GUI.color = map[index] ? NoObstacleColor : ObstacleColor;
                    BasicEditorTools.ChangeCheck_Begin();
                    BasicEditorTools.Button(map[index] ? "X" : "O", () => { map[index] = !map[index]; });
                    if(BasicEditorTools.ChangeCheck_End())
                    {
                        BasicDrawEditorTools.Repaint();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            BasicEditorTools.Box_Close();
            BasicEditorTools.Scroll_End();
            GUI.color = defaultColor;
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
            result.length = lenght;
            result.heightRate = heightRate;
            result.maxHeight = maxHeight;
            result.setConstructionFactor(constructionFactor);
            if (result.Create(ref map, false))
            {
                Undo.SetCurrentGroupName("Generate terrain - Quadrilateral");
                int group = Undo.GetCurrentGroup();
                Undo.RecordObject(terrainManager.gameObject, "Generate terrain - Quadrilateral");
                Undo.RecordObject(terrainManager, "Generate terrain - Quadrilateral");

                var array = terrainManager.TerrainLand_GenerateQuadrilateral(result, autoIntegration);
                Undo.RecordObject(terrainManager.terrainData, "Generate terrain - Quadrilateral");
                foreach (var node in array)
                {
                    if (node == null) continue;
                    Undo.RegisterCreatedObjectUndo(node, "Generate terrain - Quadrilateral");
                }
                Undo.CollapseUndoOperations(group);
            }
            onClose();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope())
            {
                drawLand();
            }
        }
        private void drawLand()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    bool isExist = !map[getMapIndex(i, j, lenght)];
                    if (!isExist) continue;
                    Vector3 startPosition = 2 * i * Vector3.forward * Obstacle.SIZE
                        + 2 * j * Vector3.right * Obstacle.SIZE;
                    Handles.DrawWireCube(startPosition, 2 * Obstacle.SIZE * Vector3.one);
                }
            }
        }
        private int getMapIndex(int i, int j, int _width) => i * _width + j;

        #endregion
        private void onClose()
        {
            terrainManagerObject = null;
            this.Close();
        }
    }
}
#endif