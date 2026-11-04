#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Obstacle))]
    public class ObstacleEditor : Editor
    {
        #region variable
        protected Obstacle Target => (Obstacle)target;
        private static int tabIndex;
        #endregion
        #region Functions
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Settings",Params),
                new BasicEditorTools.StringActionStruct("Events",Events),
            });
            serializedObject.ApplyModifiedProperties();
        }
        int obstacleTypeIndex;
        private void Params()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.ChangeCheck_Begin();
            obstacleTypeIndex = (int)Target.obstacleType;
            BasicEditorTools.DropDownList("ObstacleType", ref obstacleTypeIndex, System.Enum.GetNames(typeof(ObstacleType)));
            if (BasicEditorTools.ChangeCheck_End())
            {
                Target.ObstacleTypeChangingFunctions((ObstacleType)obstacleTypeIndex);
                if (Target.terrainManager != null)
                {
                    Target.SetMaterial(Target.terrainManager.GetMaterial((ObstacleType)obstacleTypeIndex));
                }
                BasicEditorTools.DirtyObject(Target.transform);
            }
            //
            BasicEditorTools.PropertyField(serializedObject, "fixPositionToGroundOnStart", "Fix position to Ground on start");
            //
            if (Target.obstacleType.IsClimbable())
            {
                BasicEditorTools.PropertyField(serializedObject, "_isClimbable", "isClimbable");
            }
            else
            {
                BasicEditorTools.Label("(ObstacleType: " + ((ObstacleType)obstacleTypeIndex).ToString() + ") is not climable.");
            }
            BasicEditorTools.Box_Close();
        }
        private void Events()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "OnObjectIsStandingOnMeEvent", "onStandingOnMe");
            BasicEditorTools.Box_Close();
        }
        #endregion

    }
}
#endif