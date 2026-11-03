#if UNITY_EDITOR
using HashGame.CubeWorld.InputManager;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(InputController))]
    public class InputControllerEditor : Editor
    {
        #region variable
        protected InputController Target => (InputController)target;
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
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "controllerType", "Controller type");
            if (Target.controllerType == ControllerType.Human)
            {
                BasicEditorTools.PropertyField(serializedObject, "_settings", InputControllerSettings.NAME);
                if (Target._settings)
                {
                    BasicEditorTools.ScriptableObject(Target._settings, typeof(InputControllerSettingsEditor));
                }
            }
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif