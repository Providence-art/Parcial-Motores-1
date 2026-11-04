using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DestructibleObstacle : ObstacleTypesBase
    {
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.Destructible;
        [HideInInspector] public IDestructibleObstacleBase[] destructiblesArray = new IDestructibleObstacleBase[System.Enum.GetValues(typeof(DestructibleStates)).Length];
        public DestructibleStates currentState { get; protected set; }
        public DestructibleLocalSettings localSettings = new DestructibleLocalSettings();
        public DestructibleSettings settings;
        public DestructibleSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    if ((settings = DestructibleSettings.Instance) == null)
                    {
                        settings = new DestructibleSettings();
                    }
                }
                return settings;
            }
        }
        public DestructibleBuffer buffer = new DestructibleBuffer();
        public DestructibleLocalEvents events = new DestructibleLocalEvents();
        #endregion
        private void OnValidate()
        {
            if (!settings)
            {
                settings = DestructibleSettings.Instance;
            }
        }
        private void Awake()
        {
            destructiblesArray[(int)DestructibleStates.Idle] = new DestructibleObstacle_Idle(this);
            destructiblesArray[(int)DestructibleStates.Destroy] = new DestructibleObstacle_Destroy(this);
            ChangeState(DestructibleStates.Idle, true);
        }
        private void Start()
        {
            buffer.touchCount = 0;
        }
        private void FixedUpdate()
        {
            destructiblesArray[(int)currentState].onFixUpdate();
        }
        public void ChangeState(DestructibleStates state, bool force = false)
        {
            if (!force && currentState == state) { return; }
            currentState = state;
            destructiblesArray[(int)currentState].onStart();
        }
        public override void ObjectIsStanding(GameObject obj) => destructiblesArray[(int)currentState].ObjectIsStanding(obj);

        public bool isTouchingOverflow() => buffer.touchCount >= localSettings.MaxTouchCount;

        public override void OnDestroyClass()
        {
        }
        #region struct
        [System.Serializable]
        public class DestructibleBuffer
        {
            public int touchCount = 0;
        }
        [System.Serializable]
        public class DestructibleLocalSettings : StepsEventStructBase
        {
            public int MaxTouchCount = 2;
            [Min(0.0f)] public float DestroyTime = 0.0f;
        }
        [System.Serializable]
        public class DestructibleLocalEvents : StepsEventStructBase
        {
            public UnityEvent onHeroDetection;
            public UnityEvent onDestroy;
            public void onHeroDetectionInvoke() => invoke(ref onHeroDetection);
            public void onDestroyInvoke() => invoke(ref onDestroy);
        }
        #endregion
    }
}