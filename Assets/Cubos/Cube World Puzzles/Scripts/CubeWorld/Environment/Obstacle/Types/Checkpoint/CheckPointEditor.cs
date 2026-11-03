#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;

namespace HashGame.CubeWorld.EditorTools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointEditor : Editor
    {
        #region variable
        public CheckPoint Target => (CheckPoint)target;
        private static int tabIndex;
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
                new BasicEditorTools.StringActionStruct("Events",showEvents),
                new BasicEditorTools.StringActionStruct("Information",showInfo),
            });
        }
        private void showInfo()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("State", Target.currentState.ToString());
            BasicEditorTools.Box_Close();
        }
        private void showEvents()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif