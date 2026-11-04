#if UNITY_EDITOR
using HashGame.CubeWorld.HeroManager;
using UnityEditor;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(AutoWalkSettings))]
    public class AutoWalkSettingsEditor : Editor
    {
        #region variable
        public AutoWalkSettings Target=>(AutoWalkSettings)target;
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
            BasicEditorTools.PropertyField(serializedObject, "MovementCommandMode", "Command mode", "This command is for the time that determines the motion of the system.");
            if (Target.MovementCommandMode == OptimizedCube.AutoWalk.AutowalkMovementCommandMode.OnTiming)
            {
                BasicEditorTools.PropertyField(serializedObject, "idleTime", "Idle time");
            }
            BasicEditorTools.PropertyField(serializedObject, "autowalkLoadHeroMode", "Load mode", "This command is Specifies what object to carry.");
            BasicEditorTools.PropertyField(serializedObject, "Speed", "Speed");
            BasicEditorTools.PropertyField(serializedObject, "backToStartAfterUnloading", "Back to start after unloading");
            //
            BasicEditorTools.Box_Open("Traveling");
            BasicEditorTools.PropertyField(serializedObject, "autowalkTravelCountMode", "Travel count mode");
            if (Target.autowalkTravelCountMode == LimitUnlimitEnum.Limited)
            {
                BasicEditorTools.PropertyField(serializedObject, "MaxTravelCount", "Max travel count");
            }
            BasicEditorTools.Box_Close();
            //
            BasicEditorTools.Box_Close();
        }
        #endregion
    }
}
#endif