#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(SpawnerButton))]
    public class SpawnerButtonEditor : Editor
    {
        #region variable
        protected SpawnerButton Target => (SpawnerButton)target;
        private static int tabIndex;
        protected Spawner spawner;
        #endregion

        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Local settings",Params),
                new BasicEditorTools.StringActionStruct("Events",EventsShow),
            });
            serializedObject.ApplyModifiedProperties();
        }
        private void EventsShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        private void Params()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "Target", "Target (Spawner)");
            BasicEditorTools.PropertyField(serializedObject, "activeHeroType", "Active hero type");
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif