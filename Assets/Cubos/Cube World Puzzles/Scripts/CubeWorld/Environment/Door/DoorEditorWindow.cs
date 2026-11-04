#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class DoorEditorWindow : EditorWindow
    {
        public static Door door;
        private Vector2 scrollPosition;
        private static bool highlighting = true;
        public static Color highlightColor = Color.red;
        private int tabIndex;
        private const float Scale = 1.1f;
        public const string NAME = Door.NAME + " data";
        //
        public static void OpenWindow(Door target)
        {
            door = target;
            GetWindow<DoorEditorWindow>(NAME);
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
            BasicEditorTools.Scroll_Begin(ref scrollPosition);
            doorGUI();
            BasicEditorTools.Scroll_End();
            BasicEditorTools.Button("Close", onClose);
        }
        private void doorGUI()
        {
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]{
                new BasicEditorTools.StringActionStruct("Door", doorOnGUI),
                new BasicEditorTools.StringActionStruct("Door type", doorType),
                new BasicEditorTools.StringActionStruct("OnGUI", obstacleOnGUI),
            });
        }
        private void doorType()
        {
            BasicEditorTools.Line();
            if (!door.doorTypeBase)
            {
                BasicEditorTools.Box_Open();
                BasicEditorTools.Label("This Object has no DoorType class.");
                BasicEditorTools.Box_Close();
                return;
            }
            BasicEditorTools.Box_Open();
            BasicEditorTools.ScriptableObject(door.doorTypeBase);
            BasicEditorTools.Box_Close();
        }
        private void doorOnGUI()
        {
            BasicEditorTools.Line();
            BasicEditorTools.Box_Open();
            BasicEditorTools.ScriptableObject(door, typeof(DoorEditor));
            BasicEditorTools.Box_Close();
        }
        private void obstacleOnGUI()
        {
            BasicEditorTools.Line();
            BasicEditorTools.Box_Open();
            BasicEditorTools.ChangeCheck_Begin();
            highlighting = BasicEditorTools.Toggle("Highlight obstacle", highlighting);
            highlightColor = BasicEditorTools.ColorField("Highlight Color", highlightColor);
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicEditorTools.DirtyObject(door.transform);
            }
            BasicEditorTools.Box_Close();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope(highlightColor))
            {
                drawObstacle(door.Position, door.RealSize, door.name);
            }
        }
        private void drawObstacle(Vector3 position, Vector3 size, string name = null)
        {
            Vector3 _size = Scale * size;
            Handles.DrawWireCube(position, _size);
            if (string.IsNullOrEmpty(name)) return;
            Vector3 p1 = position + Vector3.back * size.z / 2 + Vector3.up * size.y / 2 + Vector3.left * size.x / 2;
            Vector3 p2 = p1 + Vector3.up * size.y;
            Handles.DrawLine(p1, p2);
            Handles.Label(p2, name);
        }
        #endregion
        private void onClose()
        {
            door = null;
            this.Close();
        }
    }
}
#endif