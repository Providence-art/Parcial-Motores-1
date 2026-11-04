#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;

namespace HashGame.CubeWorld.EditorTools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Portal))]
    public class PortalEditor : Editor
    {
        #region variable
        protected Portal Target => (Portal)target;
        protected Portal.PortalSettings settings => Target.settings;
        protected Portal.PortalEvents events => Target.events;
        protected Portal.DestinationPortalSettings destinationPortalSettings => Target.destinationPortalSettings;
        private static int tabIndex = 0;
        #endregion
        #region Functions
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI()
        {
            drawPath();
        }
        private void drawPath()
        {
            if (!Target.editor_drawPath) return;
            if (!destinationPortalSettings.IsDestinationValid())
            {
                BasicDrawEditorTools.Lable(Target.transform.position, "No destination found.");
                return;
            }
            if (destinationPortalSettings.getDestinationTransform(out var result))
            {
                Handles.DrawLine(Target.transform.position, result.position);
            }
        }
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Params();
            serializedObject.ApplyModifiedProperties();
        }
        private void Params()
        {
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[] {
                new BasicEditorTools.StringActionStruct("Destination",showDestinationPortalSettings),
                new BasicEditorTools.StringActionStruct("Portal",showSettings),
                new BasicEditorTools.StringActionStruct("Events",showEvents),
                new BasicEditorTools.StringActionStruct("Information",showInfo),
                new BasicEditorTools.StringActionStruct("Scene UI",showSceneUI),
            });
        }
        private void showInfo()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("State", Target.currentState.ToString());
            BasicEditorTools.Box_Close();
        }
        private void showSceneUI()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "editor_drawPath", "Draw path");
            BasicEditorTools.Box_Close();
        }
        private void showEvents()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Portal events");
            BasicEditorTools.Box_Close();
        }
        private void showSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "settings", "Portal settings");
            BasicEditorTools.Box_Close();
        }
        private void showDestinationPortalSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "destinationPortalSettings.useFreeTransform", "Use free transform");
            if (destinationPortalSettings.useFreeTransform)
            {
                BasicEditorTools.PropertyField(serializedObject, "destinationPortalSettings.destinationTransform", "Destination transform");
            }
            else
            {
                BasicEditorTools.PropertyField(serializedObject, "destinationPortalSettings.portalDestination", "Portal destination");
            }
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif