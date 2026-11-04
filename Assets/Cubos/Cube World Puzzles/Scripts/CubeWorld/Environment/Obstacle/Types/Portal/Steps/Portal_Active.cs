using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Portal_Active : IPortalBase
    {
        public Portal_Active(Portal controller) : base(PortalStates.Active, controller) { }
        private float t;
        private bool isLock;
        public override bool CanHostReception(GameObject obj) => false;
        public override void OnStepStart()
        {
            Reset();
            Controller.events.onActiveInvoke();
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null)
                Controller.obstacle.terrainManager.PlaySFX(Controller.MyObstacleType);
        }
        public override void onFixedUpdate()
        {
            t += Time.deltaTime;
            if (t > Controller.settings.ActiveTime) { isLock = false; }
            if (isLock || Controller.hero.currentStep != HeroSteps.Idle) return;
            ChangeState(PortalStates.Teleport);
        }
        public override void OnTriggerEnter(Collider other)
        {
        }
        private void Reset()
        {
            t = 0.0f;
            isLock = true;
        }
    }
}