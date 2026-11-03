#if UNITY_EDITOR
using HashGame.CubeWorld.HeroManager;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(HeroController))]
    public class HeroControllerEditor : Editor
    {
        #region variable
        protected HeroController Target => (HeroController)target;
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
                new BasicEditorTools.StringActionStruct("Local settings",localSettings),
                new BasicEditorTools.StringActionStruct("Settings",settings),
                new BasicEditorTools.StringActionStruct("Buffer",showBuffer),
                new BasicEditorTools.StringActionStruct("Events",showEvents),
                new BasicEditorTools.StringActionStruct("AudioClips",showAudioClips),
            });
        }
        private void showAudioClips()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "_audioClips", HeroAudioSettings.NAME);
            if (Target._audioClips)
            {
                BasicEditorTools.ScriptableObject(Target._audioClips);
            }
            BasicEditorTools.Box_Close();
        }
        private void showEvents()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        private void showBuffer()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "buffer", "Buffer");
            BasicEditorTools.Box_Close();
        }
        private void settings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "_settings", HeroSettings.NAME);
            if (Target._settings)
            {
                BasicEditorTools.ScriptableObject(Target._settings);
            }
            BasicEditorTools.Box_Close();
        }
        private void localSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "isRespawnable", "Respawnable");
            BasicEditorTools.PropertyField(serializedObject, "CubeWheel", "Hero Wheel");
            BasicEditorTools.PropertyField(serializedObject, "Head", "Hero Head");
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif