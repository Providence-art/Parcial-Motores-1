using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Undercover : ObstacleTypesBase
    {
        #region varibale
        public override ObstacleType MyObstacleType => ObstacleType.Undercover;
        [HideInInspector] public IUndercover[] undercoverArray = new IUndercover[System.Enum.GetValues(typeof(UndercoverStates)).Length];
        public UndercoverStates currentState { get; protected set; }
        public UndercoverEvents events = new UndercoverEvents();
        #endregion
        #region Functions
        private void Awake()
        {
            undercoverArray[(int)UndercoverStates.Init] = new Undercover_Init(this);
            undercoverArray[(int)UndercoverStates.Stealth] = new Undercover_Stealth(this);
            undercoverArray[(int)UndercoverStates.Identify] = new Undercover_Identify(this);
        }
        private void Start()
        {
            ChangeState(UndercoverStates.Init, true);
        }
        #endregion
        #region functions
        public void ChangeState(UndercoverStates state, bool force = false)
        {
            if (currentState == state && !force) return;
            undercoverArray[(int)currentState].OnStepFinish();
            undercoverArray[(int)(currentState = state)].OnStepStart();
        }

        public override void ObjectIsStanding(GameObject obj)
        {
            undercoverArray[(int)currentState].ObjectIsStanding(obj);
        }

        public override void OnDestroyClass()
        {
        }
        #endregion
        #region struct
        [System.Serializable]
        public class UndercoverEvents : StepsEventStructBase
        {
            public UnityEvent onBeingIdentified;
            public void onBeingIdentifiedInvoke() => invoke(ref onBeingIdentified);
        }
        #endregion
    }
}