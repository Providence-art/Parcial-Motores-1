#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    using HashGame.CubeWorld.HeroManager;
    using UnityEditor;
    using UnityEngine;
    [CustomEditor(typeof(HeroLineSensorController))]
    public class HeroLineSensorControllerEditor : Editor
    {
        #region variable
        protected HeroLineSensorController Target;
        #endregion
        #region Functions
        private void OnEnable()
        {
            Target = (HeroLineSensorController)target;
        }
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            BasicEditorTools.Line();
            serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            ShowRays();
            ShowSize();
        }
        private void ShowRays()
        {
            if (!Target.editor_showRays) return;
            Handles.color = Target.editor_rayColor;
            DrawRay("Up", Target.Position, Vector3.up, Target.getMaxDistance(Vector3.up));
            DrawRay("Forward", Target.Position, Vector3.forward, Target.getMaxDistance(Vector3.forward));
            DrawRay("Back", Target.Position, Vector3.back, Target.getMaxDistance(Vector3.back));
            DrawRay("Right", Target.Position, Vector3.right, Target.getMaxDistance(Vector3.right));
            DrawRay("Left", Target.Position, Vector3.left, Target.getMaxDistance(Vector3.left));
            DrawRay("isGrounded", Target.Position, Vector3.down, Target.getMaxDistance(Vector3.down));
            //
            DrawRay("Up Forward", Target.UpPosition, Vector3.forward, Target.getMaxDistance(Vector3.forward));
            DrawRay("Up Back", Target.UpPosition, Vector3.back, Target.getMaxDistance(Vector3.back));
            DrawRay("Up Right", Target.UpPosition, Vector3.right, Target.getMaxDistance(Vector3.right));
            DrawRay("Up Left", Target.UpPosition, Vector3.left, Target.getMaxDistance(Vector3.left));
            //
            DrawRay("Forward Ground", Target.ForwardGroundPosition, Vector3.down, Target.getMaxDistance(Vector3.down));
            DrawRay("Backward Ground", Target.BackwardGroundPosition, Vector3.down, Target.getMaxDistance(Vector3.down));
            DrawRay("Right Ground", Target.RightGroundPosition, Vector3.down, Target.getMaxDistance(Vector3.down));
            DrawRay("Left Ground", Target.LeftGroundPosition, Vector3.down, Target.getMaxDistance(Vector3.down));
        }
        private void ShowSize()
        {
            if (!Target.editor_showSize) return;
            Handles.color = Target.editor_rayColor;
            DrawRay("Up", Target.Controller.Position, Vector3.up, Target.Controller.UpDownSize);
        }
        #endregion
        #region functions
        private void DrawRay(string name, Vector3 position, Vector3 direction, float size)
        {
            Vector3 p = position + direction * Target.getMaxDistance(direction);
            Handles.DrawLine(position, p);
            Handles.Label(p, name);
        }
        #endregion
    }
}
#endif