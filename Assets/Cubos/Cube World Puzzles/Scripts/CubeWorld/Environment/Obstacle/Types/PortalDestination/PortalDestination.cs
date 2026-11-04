using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;
namespace HashGame.CubeWorld.OptimizedCube
{
    public class PortalDestination : ObstacleTypesBase
    {
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.PortalDestination;
        #region portal
        [HideInInspector] public HeroController hero;
        protected PortalStates currentState;
        [HideInInspector] public IPortalBase[] portalArray = new IPortalBase[System.Enum.GetValues(typeof(PortalStates)).Length];
        #endregion
        [HideInInspector] public Rigidbody rb;
        #endregion
        private void OnValidate()
        {
        }
        private void Awake()
        {
            if (obstacleType != MyObstacleType)
            {
                BasicExtensions.DestroyObject(this);
            }
            if ((rb = GetComponent<Rigidbody>()) == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            boxCollider.isTrigger = true;
            boxCollider.size *= MiniaturizedBoxColliderScale;
        }
        public override void ObjectIsStanding(GameObject obj) { }

        public bool IsEmptyInsidePortalDestination()
        {
            Vector3 center = transform.position;
            Vector3 halfExtents = boxCollider.size / 2f;
            Quaternion rotation = boxCollider.transform.rotation;

            Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, rotation);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null || colliders[i].gameObject == null) continue;
                GameObject obj = colliders[i].gameObject;
                if (obj.GetComponent<HeroController>()) return false;
                if (obj.GetComponent<Obstacle>()) return false;
            }
            return true;
        }

        public override void OnDestroyClass()
        {
            BasicExtensions.DestroyObject(rb);
        }
    }
}