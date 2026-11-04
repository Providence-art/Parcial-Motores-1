#if UNITY_EDITOR
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(TerrainEditorDataStore))]
    public class TerrainEditorDataStoreEditor : Editor
    {
        #region variable
        protected TerrainEditorDataStore Target => (TerrainEditorDataStore)target;
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BasicEditorTools.PropertyField(serializedObject, "settings", "Settings");
            if (Target.settings != null)
            {
                BasicEditorTools.ScriptableObject(Target.settings);
            }
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
#endif