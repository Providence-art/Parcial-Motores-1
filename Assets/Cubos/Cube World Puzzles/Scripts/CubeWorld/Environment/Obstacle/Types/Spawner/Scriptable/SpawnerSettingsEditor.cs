#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(SpawnerSettings))]
    public class SpawnerSettingsEditor : Editor
    {
        #region variable
        protected SpawnerSettings Target => (SpawnerSettings)target;
        #endregion
        #region Functions
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
            BasicEditorTools.Box_Open("Generate");
            BasicEditorTools.PropertyField(serializedObject, "GenerateType", "Generate type");
            if (Target.GenerateType == LimitUnlimitEnum.Limited)
            {
                BasicEditorTools.PropertyField(serializedObject, "MaxGenerateCount", "Max generate count");
            }
            BasicEditorTools.PropertyField(serializedObject, "MaxObjectGenerateAtOnce", "Max object(s) generate at once");
            BasicEditorTools.Box_Close();

            BasicEditorTools.Box_Open("Cooldown");
            BasicEditorTools.PropertyField(serializedObject, "coolDownTime", "Cooldown time");
            BasicEditorTools.PropertyField(serializedObject, "useColors", "Use colors");
            if (Target.useColors)
            {
                BasicEditorTools.PropertyField(serializedObject, "onReadyColor", "On ready");
                BasicEditorTools.PropertyField(serializedObject, "onCoolDownColor", "On cooldown");
            }
            BasicEditorTools.Box_Close();

            BasicEditorTools.Box_Open("Prefab");
            BasicEditorTools.PropertyField(serializedObject, "PrefabAutoConfig", "Prefab auto config");
            BasicEditorTools.PropertyField(serializedObject, "PrefabOrderType", "Prefabs order generate type");
            BasicEditorTools.PropertyField(serializedObject, "HeroPrefabs", "Hero prefabs");
            BasicEditorTools.Button("Load hero prefab(s)", () => { Target.LoadPrefabs(); });
            BasicEditorTools.Box_Close();
        }
        #endregion

    }
}
#endif