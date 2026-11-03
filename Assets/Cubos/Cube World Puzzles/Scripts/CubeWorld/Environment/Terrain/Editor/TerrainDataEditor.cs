#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(TerrainData))]
    public class TerrainDataEditor : Editor
    {
        #region variable
        protected TerrainData Target;
        #endregion
        #region Functions
        private void OnEnable()
        {
            Target = (TerrainData)target;
        }
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            BasicEditorTools.Line(2);
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
#endif