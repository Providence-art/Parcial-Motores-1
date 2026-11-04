using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Portal : ObstacleTypesBase
    {
        #region enum
        public enum PortalActiveHeroType : int
        {
            Human,
            Cpu,
            All,
        }
        #endregion
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.Portal;
        public DestinationPortalSettings destinationPortalSettings = new DestinationPortalSettings();
        public PortalSettings settings = new PortalSettings()
        {
            portalActiveHeroType = PortalActiveHeroType.Human,
        };
        public PortalEvents events = new PortalEvents();
        #region portal
        [HideInInspector] public HeroController hero;
        public PortalStates currentState { get; protected set; }
        [HideInInspector] public IPortalBase[] portalArray = new IPortalBase[System.Enum.GetValues(typeof(PortalStates)).Length];
        #endregion
        [HideInInspector] public Rigidbody rb;
        #region editor
#if UNITY_EDITOR
        public bool editor_drawPath = true;
#endif
        #endregion
        #endregion
        #region Functions
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
            portalArray[(int)PortalStates.Idle] = new Portal_Idle(this);
            portalArray[(int)PortalStates.Active] = new Portal_Active(this);
            portalArray[(int)PortalStates.Teleport] = new Portal_Teleport(this);
            ChangeState(PortalStates.Idle, true);
            boxCollider.size *= MiniaturizedBoxColliderScale;
        }
        private void FixedUpdate()
        {
            portalArray[(int)currentState].onFixedUpdate();
        }
        private void OnTriggerEnter(Collider other)
        {
            portalArray[currentState.ToIndex()].OnTriggerEnter(other);
        }
        #endregion
        #region functions
        public void ChangeState(PortalStates state, bool force = false)
        {
            if (currentState == state && !force) return;
            currentState = state;
            portalArray[(int)currentState].OnStepStart();
        }
        public bool CanHostReception(GameObject obj)
        {
            if (!CanHostReception()) return false;
            return portalArray[(int)currentState].CanHostReception(obj);
        }
        public bool CanHostReception()
        {
            if (!destinationPortalSettings.IsDestinationValid()) return false;
            if (!destinationPortalSettings.useFreeTransform && !destinationPortalSettings.portalDestination.IsEmptyInsidePortalDestination()) return false;
            return true;
        }
        public override void ObjectIsStanding(GameObject obj)
        {
            if (obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (hero == null) return;
            if (hero.IsHuman) return;
            hero.ForceToDestroyPhase();
        }
        public void Teleport()
        {
            if (hero == null || transform == null) return;
            if (destinationPortalSettings.getDestinationTransform(out var _transform))
            {
                hero.transform.SetPositionAndRotation(_transform.position, _transform.rotation);
            }
            hero.TeleportingIsFinish();
            hero = null;
        }

        public override void OnDestroyClass()
        {
            BasicExtensions.DestroyObject(rb);
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct DestinationPortalSettings
        {
            public bool useFreeTransform;
            public Transform destinationTransform;
            public PortalDestination portalDestination;
            public bool IsDestinationValid()
            {
                return useFreeTransform && (destinationTransform != null) || portalDestination != null;
            }
            public bool getDestinationTransform(out Transform result)
            {
                if (IsDestinationValid())
                {
                    result = useFreeTransform ? destinationTransform : portalDestination.transform;
                    return result != null;
                }
                result = default(Transform);
                return false;
            }
        }
        [System.Serializable]
        public struct PortalSettings
        {
            [Min(0.0f)]
            public float ActiveTime;
            public PortalActiveHeroType portalActiveHeroType;
        }
        [System.Serializable]
        public class PortalEvents : StepsEventStructBase
        {
            public UnityEvent onActive;
            public UnityEvent onTeleport;
            public void onActiveInvoke() => invoke(ref onActive);
            public void onTeleportInvoke() => invoke(ref onTeleport);
        }
        #endregion
    }
}