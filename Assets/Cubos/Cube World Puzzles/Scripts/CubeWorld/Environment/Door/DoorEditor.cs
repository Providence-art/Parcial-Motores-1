#if UNITY_EDITOR
using UnityEditor;
using HashGame.CubeWorld.OptimizedCube;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(Door))]
    public class DoorEditor : Editor
    {
        #region variable
        public Door Target => (Door)target;
        private static int tabIndex;
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Settings",settings),
                new BasicEditorTools.StringActionStruct("Events",events),
            });
            serializedObject.ApplyModifiedProperties();
        }
        int doorTypeIndex;
        private string[] dropdownListNames=System.Enum.GetNames(typeof(DoorType));
        private void settings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.ChangeCheck_Begin();
            doorTypeIndex = (int)Target.doorType;
            BasicEditorTools.DropDownList("Door type", ref doorTypeIndex, dropdownListNames);
            if (BasicEditorTools.ChangeCheck_End())
            {
                Target.SetDoorType((DoorType)doorTypeIndex);
                BasicEditorTools.DirtyObject(Target.transform);
            }
            BasicEditorTools.PropertyField(serializedObject, "settings", "Settings");
            BasicEditorTools.Box_Close();
        }
        private void events()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif