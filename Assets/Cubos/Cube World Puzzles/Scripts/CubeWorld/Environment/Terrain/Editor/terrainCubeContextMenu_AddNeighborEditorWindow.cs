#if UNITY_EDITOR
using System;
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class terrainCubeContextMenu_AddNeighborEditorWindow : EditorWindow
    {
        #region variable
        public const string NAME = "Add obstacle(s)";
        int face = (int)CubeFace.Forward;
        public int Count = 1;
        public static GameObject terrainManagerObject;
        public static Obstacle[] obstacles;
        #endregion
        public static void OpenWindow(Obstacle[] node, GameObject TerrainManagerObject)
        {
            obstacles = node;
            terrainManagerObject = TerrainManagerObject;
            GetWindow<terrainCubeContextMenu_AddNeighborEditorWindow>(NAME);
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
            BasicEditorTools.ChangeCheck_Begin();
            Count = Math.Max(1, EditorGUILayout.IntField("Count", Count));
            if (Count > 50) Count = 50;
            BasicEditorTools.DropDownList("Obstacle side", ref face, Enum.GetNames(typeof(CubeFace)));
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicDrawEditorTools.Repaint();
            }
            BasicEditorTools.Buttons(new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Close",()=>{ this.Close(); }),
                new BasicEditorTools.StringActionStruct("Generate",generate)
            }, 2);
        }
        private void generate()
        {
            TerrainManager terrainManager;
            if (terrainManagerObject == null
                || (terrainManager = terrainManagerObject.GetComponent<TerrainManager>()) == null
                || (obstacles) == null)
            {
                onClose();
                return;
            }
            foreach (Obstacle obstacle in obstacles)
            {
                Undo.SetCurrentGroupName(NAME);
                int group = Undo.GetCurrentGroup();
                Undo.RecordObject(terrainManager.gameObject, NAME);
                Undo.RecordObject(terrainManager, NAME);
                Undo.RecordObject(terrainManager.terrainData, NAME);
                var array = terrainManager.GenerateInLine(obstacle, (CubeFace)face, Count);
                foreach (var node in array)
                {
                    if (node == null) continue;
                    Undo.RegisterCreatedObjectUndo(node, NAME);
                }
                Undo.CollapseUndoOperations(group);
            }
            //
            onClose();
        }
        private void onClose()
        {
            terrainManagerObject = null;
            obstacles = null;
            this.Close();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope(ObstacleEditorWindow.highlightColor))
            {
                onSceneGUIFunctions();
            }
        }
        private void onSceneGUIFunctions()
        {
            if (obstacles == null) return;
            foreach (var obstacle in obstacles)
            {
                if (obstacle == null) continue;
                Vector3 point = obstacle.transform.position;
                for (int i = 0; i <= Count; i++)
                {
                    Handles.DrawWireCube(point, obstacle.RealSize);
                    point += Obstacle.getDirection((CubeFace)face) * (2 * obstacle.getSize((CubeFace)face));
                }
            }
        }
        #endregion
    }
}
#endif