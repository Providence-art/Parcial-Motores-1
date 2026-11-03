using UnityEngine;
namespace HashGame.CubeWorld.CameraSystem
{
    public class IsometricCameraController : CameraBasic
    {
        #region variable
        #region read only
        public const string NAME = "Isometric camera";
        public override bool IncludeAxisMapping => false;
        #endregion
        [Range(-360, 360)]
        public float RotationAngle = -150f;
        #endregion
        void LateUpdate()
        {
            if (target == null) return;
            transform.position = target.transform.position + offset(RotationAngle, distance, height);
            transform.LookAt(target.transform);
        }

        public override void ChangeRotation(Vector3 motionAxis)
        {
        }
    }
}