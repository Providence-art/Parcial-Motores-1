#if UNITY_EDITOR
using HashGame.CubeWorld.InputManager;
using UnityEditor;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(InputControllerSettings))]
    public class InputControllerSettingsEditor : Editor
    {
        #region variable
        protected InputControllerSettings Target => (InputControllerSettings)target;
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
            BasicEditorTools.PropertyField(serializedObject, "onFrequencySampling", "On frequency sampling");
            BasicEditorTools.PropertyField(serializedObject, "inputType", "InputType");
            if (Target.inputType == InputType.KeyBoard)
            {
                BasicEditorTools.PropertyField(serializedObject, "keyBoardInput", "Keyboard Input(s)");
            }
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif