#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        #region variable
        protected Spawner Target => (Spawner)target;
        private static int tabIndex;
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
                new BasicEditorTools.StringActionStruct("Settings",SettingsTab),
                new BasicEditorTools.StringActionStruct("Events",EventsTab),
                new BasicEditorTools.StringActionStruct("Buffer",BufferTab),
                new BasicEditorTools.StringActionStruct("Information",InformationTab),
            });
        }
        private void InformationTab()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("Step", Target.currentState.ToString());
            BasicEditorTools.Box_Close();
        }
        private void BufferTab()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "buffer", "Buffer");
            BasicEditorTools.Box_Close();
        }
        private void EventsTab()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "events");
            BasicEditorTools.Box_Close();
        }
        private void SettingsTab()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "_settings", SpawnerSettings.NAME);
            if (Target._settings != null)
            {
                BasicEditorTools.ScriptableObject(Target._settings, typeof(SpawnerSettingsEditor));
            }
            BasicEditorTools.Box_Close();
        }
        #endregion

    }
}
#endif