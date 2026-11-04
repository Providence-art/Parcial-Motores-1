namespace HashGame.CubeWorld.HeroManager
{
    using HashGame.CubeWorld.OptimizedCube;
    using UnityEngine;

    [RequireComponent(typeof(HeroController))]
    public class HeroLineSensorController : MonoBehaviour
    {
        #region variable
        [Range(1.0f, 1.0f)]
        public float depth = 1.0f;
        #region read only
        public Vector3 Position { get => transform.position; }
        public Vector3 UpPosition { get => Position + Vector3.up * Size_Y; }
        public Vector3 ForwardGroundPosition { get => Position + Vector3.forward * Size_Z; }
        public Vector3 BackwardGroundPosition { get => Position + Vector3.back * Size_Z; }
        public Vector3 RightGroundPosition { get => Position + Vector3.right * Size_X; }
        public Vector3 LeftGroundPosition { get => Position + Vector3.left * Size_X; }
        public float Size_X { get => Controller.HeroSize.x; }
        public float Size_Y { get => Controller.HeroSize.y; }
        public float Size_Z { get => Controller.HeroSize.z; }
        #endregion
        private bool isGrounded = false;
        public bool IsGrounded { get => isGrounded; }
        #region hide
        [HideInInspector] public HeroController Controller;
        #endregion
        #region editor
#if UNITY_EDITOR
        public bool editor_showRays = false;
        public bool editor_showSize = false;
        public Color editor_rayColor = Color.green;
#endif
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            Controller = GetComponent<HeroController>();
        }
        #endregion
        #region functions
        #region basic
        public RaycastHit[] Radiation_RaycastAll(Vector3 direction)
        {
            return Physics.RaycastAll(Position, direction, getMaxDistance(direction));
        }
        public bool Radiation(Vector3 direction, out RaycastHit hit)
            => Physics.Raycast(Position, direction, out hit, getMaxDistance(direction));
        public bool RadiationToGround(Vector3 direction, out RaycastHit hit) => RadiationToGround(direction, out hit, getMaxDistance(direction));
        public bool RadiationToGround(Vector3 direction, out RaycastHit hit, float maxDistance)
        {
            Vector3 position;
            if (direction == Vector3.forward)
            {
                position = ForwardGroundPosition;
            }
            else if (direction == Vector3.back)
            {
                position = BackwardGroundPosition;
            }
            else if (direction == Vector3.right)
            {
                position = RightGroundPosition;
            }
            else
            {
                position = LeftGroundPosition;
            }
            return Physics.Raycast(position, Vector3.down, out hit, maxDistance);
        }

        public float getMaxDistance(Vector3 direction)
        {
            if (direction == Vector3.down || direction == Vector3.up) return depth * Size_Y;
            if (direction == Vector3.forward || direction == Vector3.back) return depth * Size_Z;
            return depth * Size_X;
        }
        public bool UpRadiation(Vector3 direction, out RaycastHit hit)
            => Physics.Raycast(UpPosition, direction, out hit, getMaxDistance(direction));
        #endregion
        #region Ground
        public bool checkGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(Position, Vector3.down, out hit, getMaxDistance(Vector3.down)))
            {
                isGrounded = hit.collider != null && hit.collider.GetComponent<DynamicCubeFaceRendering>() != null;
            }
            return isGrounded;
        }
        public GameObject GroundFinding()
        {
            RaycastHit hit;
            if (Physics.Raycast(Position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }
        #endregion
        #region 
        public bool CanMovingInDirection(Vector3 direction)
        {
            Door door;
            if (Controller.currentStep == HeroSteps.Lock) return false;
            if (Controller.lineSensorController.Radiation(direction, out var hit))// check folow motion -> obstacle
            {
                HeroController anotherHero;
                Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    if (obstacle.obstacleType.IsClimbable() && !Controller.lineSensorController.Radiation(Vector3.up, out hit))// check up -> no obstacle
                    {
                        if (Controller.lineSensorController.UpRadiation(direction, out hit))// check follow motion -> obstacle
                        {
                            return false;
                        }
                        return true;
                    }
                    if (obstacle.obstacleType == ObstacleType.Portal)
                    {
                        Portal portal = obstacle.GetComponent<Portal>();
                        if (portal)
                        {
                            if (portal.CanHostReception(this.gameObject))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                anotherHero = hit.collider.GetComponent<HeroController>();
                if (anotherHero != null)
                {
                    return anotherHero.lineSensorController.CanMovingInDirection(direction);
                }

                door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    return false;
                }
                return true;
            }
            if (Controller.lineSensorController.RadiationToGround(direction, out hit, Mathf.Infinity))
            {
                if ((door = hit.collider.GetComponent<Door>()) != null)
                {
                    return false;
                }
            }
            else if (!Controller.Settings.EnableFalling)
            {
                return false;
            }
            return true;
        }
    }
    #endregion
    #endregion
}