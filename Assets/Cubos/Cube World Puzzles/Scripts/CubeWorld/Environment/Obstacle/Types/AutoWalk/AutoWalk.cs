using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(BoxCollider))]
    public class AutoWalk : ObstacleTypesBase
    {
        #region enum
        public enum AutowalkMovementCommandMode : int
        {
            OnHeroDetection = 0,
            OnTiming
        }
        public enum AutowalkLoadHeroType : int
        {
            Human,
            Cpu,
            All,
        }
        #endregion
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.AutoWalk;
        [HideInInspector] public Rigidbody rb;
        //
        public AutoWalkEvents events = new AutoWalkEvents();
        [HideInInspector] public BufferStruct buffer = new BufferStruct();
        public LocalSettings localSettings = new LocalSettings();
        public AutoWalkSettings settings;
        public AutoWalkSettings Settings
        {
            get
            {
                if (settings == null) return AutoWalkSettings.Instance;
                return settings;
            }
        }
        public AutoWalkStates currentState { get; protected set; }
        [HideInInspector] public IAutoWalkBase[] autoWalkArray = new IAutoWalkBase[System.Enum.GetValues(typeof(AutoWalkStates)).Length];
        #region editor
#if UNITY_EDITOR
        [HideInInspector] public int editor_TabIndex;
        [HideInInspector] public bool editor_drawDestination;
#endif
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            if (!settings)
            {
                settings = AutoWalkSettings.Instance;
            }
        }
        private void Awake()
        {
            if ((rb = GetComponent<Rigidbody>()) == null)rb = gameObject.AddComponent<Rigidbody>();
            boxCollider.size *= MiniaturizedBoxColliderScale;
            boxCollider.isTrigger = true;
            rb.isKinematic = true;
            buffer.setStartTransform(transform);
            initSates();
            ChangeState(AutoWalkStates.Init, true);
        }
        private void Start()
        {
            if (obstacleType != MyObstacleType)
            {
                BasicExtensions.DestroyObject(this);
                return;
            }
        }
        private void Update()
        {
            autoWalkArray[(int)currentState].onUpdate();
        }
        private void FixedUpdate()
        {
            autoWalkArray[(int)currentState].onFixUpdate();
        }
        private void OnDestroy()
        {
            OnDestroyClass();
        }
        private void OnTriggerExit(Collider other) => autoWalkArray[(int)currentState].OnTriggerExit(other);
        #endregion
        #region functions
        #region state
        private void initSates()
        {
            autoWalkArray[(int)AutoWalkStates.Init] = new AutoWalk_Init(this);
            autoWalkArray[(int)AutoWalkStates.Idle] = new AutoWalk_Idle(this);
            autoWalkArray[(int)AutoWalkStates.Move] = new AutoWalk_Move(this);
            autoWalkArray[(int)AutoWalkStates.Unloading] = new AutoWalk_Unloading(this);
        }
        public void ChangeState(AutoWalkStates state, bool force = false)
        {
            if (currentState == state && !force) return;
            autoWalkArray[(int)currentState].onEnd();
            events.onPhaseChangeInvoke();
            autoWalkArray[(int)(currentState = state)].onStart();
        }
        #endregion
        public override void ObjectIsStanding(GameObject obj) => autoWalkArray[(int)currentState].ObjectIsStanding(obj);
        #region TargetPosition
        public bool getTargetPosition(out Vector3 position)
        {
            if (buffer.inSpawnPosition)
            {
                if (localSettings.destination == null)
                {
                    position = default(Vector3);
                    return false;
                }
                position = localSettings.destination.transform.position;
                return true;
            }
            position = buffer.StartPosition;
            return true;
        }

        public override void OnDestroyClass()
        {
            BasicExtensions.DestroyObject(rb);
        }
        #endregion
        #endregion
        #region struct
        [System.Serializable]
        public struct BufferStruct
        {
            public Vector3 StartPosition;
            public Quaternion StartRotation;
            public HeroController Hero;
            public bool inSpawnPosition;
            public int travelCount;
            public void setStartTransform(Transform t) => setTransform(t, ref StartPosition, ref StartRotation);
            private void setTransform(Transform t, ref Vector3 position, ref Quaternion rotation)
            {
                if (t == null) return;
                position = t.position;
                rotation = t.rotation;
            }
            public bool changeTowardsSide() => inSpawnPosition = !inSpawnPosition;
            public bool isStayOnRespawnPosition() => inSpawnPosition == true;
        }
        [System.Serializable]
        public struct LocalSettings
        {
            public AutoWalkDestination destination;
        }
        [System.Serializable]
        public class AutoWalkEvents : StepsEventStructBase
        {
            public UnityEvent onPhaseChange;
            public UnityEvent onTravelingStarted;
            public UnityEvent onTravelingEnded;
            public void onPhaseChangeInvoke() => invoke(ref onPhaseChange);
            public void onTravelingStartedInvoke() => invoke(ref onTravelingStarted);
            public void onTravelingEndedInvoke() => invoke(ref onTravelingEnded);
        }
        #endregion
    }
}