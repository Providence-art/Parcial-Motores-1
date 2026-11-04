#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(DoorLever))]
    public class DoorLeverEditor : Editor
    {

        #region variable
        protected DoorLever Target => (DoorLever)target;
        private static int tabIndex;
        public static bool DrawConnections;
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            tabs();
            serializedObject.ApplyModifiedProperties();
        }
        private void tabs()
        {
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Settings",showSettings),
                new BasicEditorTools.StringActionStruct("Events",showEvents),
            });
        }
        private void showSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "activeMode", "Active mode");
            BasicEditorTools.PropertyField(serializedObject, "doors", "Door(s)");
            DrawConnections = BasicEditorTools.Toggle("Draw connection(s)", DrawConnections);
            BasicEditorTools.Box_Close();
        }
        private void showEvents()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI()
        {
            if (DrawConnections && Event.current.type == EventType.Repaint)
            {
                if (Target.doors != null)
                {
                    foreach (Door door in Target.doors)
                    {
                        Handles.DrawLine(Target.transform.position, door.transform.position);
                    }
                }
            }
        }
        #endregion
    }
}
#endif