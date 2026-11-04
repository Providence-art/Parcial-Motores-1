using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DoorLever : ObstacleTypesBase
    {
        #region enum
        public enum ActiveMode : int
        {
            OnClick = 0,
            OnHolding
        }
        #endregion
        #region variable
        public Door[] doors;
        public DoorLeverEvents events = new DoorLeverEvents();
        public ActiveMode activeMode = ActiveMode.OnClick;
        [HideInInspector] public Rigidbody rb;
        public override ObstacleType MyObstacleType => ObstacleType.DoorLever;
        #endregion
        #region Functions
        private void Awake()
        {
            if ((rb = GetComponent<Rigidbody>()) == null) rb = gameObject.AddComponent<Rigidbody>();
            boxCollider.size *= MiniaturizedBoxColliderScale;
            boxCollider.isTrigger = true;
            rb.isKinematic = true;
        }
        private void Start()
        {
            if (doors != null)
            {
                foreach (Door door in doors)
                {
                    door.doorType = DoorType.LeverOpen;
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (doors == null || doors.Length == 0 || other == null) return;
            HeroController hero = other.GetComponent<HeroController>();
            if (hero == null) return;
            foreach (Door door in doors)
            {
                if (door == null) continue;
                door.onLeverOpenGate();
            }
            events.onPressingInvoke();
        }
        private void OnTriggerExit(Collider other)
        {
            if (doors == null || doors.Length == 0 || other == null || activeMode != ActiveMode.OnHolding) return;
            HeroController hero = other.GetComponent<HeroController>();
            if (hero == null) return;
            foreach (Door door in doors)
            {
                if (door == null) continue;
                door.onLeverCloseGate();
            }
        }
        #endregion
        #region functions
        public override void ObjectIsStanding(GameObject obj)
        {
        }

        public override void OnDestroyClass()
        {
            BasicExtensions.DestroyObject(rb);
        }
        #endregion
        #region struct
        [System.Serializable]
        public class DoorLeverEvents : StepsEventStructBase
        {
            public UnityEvent onPressing;
            public void onPressingInvoke() => invoke(ref onPressing);
        }
        #endregion
    }
}