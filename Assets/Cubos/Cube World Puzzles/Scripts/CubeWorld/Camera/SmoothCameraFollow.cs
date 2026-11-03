    using UnityEngine;
    namespace HashGame.CubeWorld.CameraSystem
    {
    public class SmoothCameraFollow : CameraBasic
    {
        #region variable
        #region read only
        public const string NAME = "Smooth camera";
        public const float FWRD_ANGLE = -90f;
        public const float BACK_ANGLE = +90.0f;
        public const float LEFT_ANGLE = 0.0f;
        public const float RIGHT_ANGLE = 180.0f;
        public override bool IncludeAxisMapping => true;
        #endregion
        public float smoothTime = 0.1f;
        public float maxSpeed = 30.0f;
        #region private
        private float _angle = FWRD_ANGLE;
        private Vector3 _velocity = Vector3.zero;
        #endregion
        #endregion
        void LateUpdate()
        {
            if (target == null) return;
            Vector3 targetPosition = target.transform.position + offset(_angle, distance, height);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime, maxSpeed);
            transform.LookAt(target.transform);
        }

        public override void ChangeRotation(Vector3 motionAxis)
        {
            if (target == null) return;
            if (motionAxis == Vector3.right)
            {
                _angle = RIGHT_ANGLE;
            }
            else if (motionAxis == Vector3.left)
            {
                _angle = LEFT_ANGLE;
            }
            else if (motionAxis == Vector3.back)
            {
                _angle = BACK_ANGLE;
            }
            else
            {
                _angle = FWRD_ANGLE;
            }
        }
    }
}