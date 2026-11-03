#if UNITY_EDITOR
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(KeyController))]
    public class KeyControllerEditor : Editor
    {
        #region variable
        protected KeyController Controller => (KeyController)target;
        private int tabIndex;
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Settings",Params),
                new BasicEditorTools.StringActionStruct("Events",events),
                new BasicEditorTools.StringActionStruct("Information",information),
            });
            serializedObject.ApplyModifiedProperties();
        }


        private void Params()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "keyData", "Data");
            BasicEditorTools.PropertyField(serializedObject, "transferToTheCustomerData", "Transfering Data");
            BasicEditorTools.PropertyField(serializedObject, "heroInventoryStoreData", "InventoryStore Data");
            BasicEditorTools.Box_Close();
        }
        private void events()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        private void information()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("State", Controller.currentStep.ToString());
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif
