#if UNITY_EDITOR
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
namespace HashGame.CubeWorld.EditorHandler
{
    [CustomEditor(typeof(TerrainLayout))]
    public class TerrainLayoutEditor : Editor
    {
        #region variable
        protected TerrainLayout Target;
        #endregion
        #region Functions
        private void OnEnable()
        {
            Target = (TerrainLayout)target;
        }
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
#endif