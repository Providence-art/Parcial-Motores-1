using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Portal_Teleport : IPortalBase
    {
        public Portal_Teleport(Portal controller) : base(PortalStates.Teleport, controller) { }

        public override bool CanHostReception(GameObject obj) => false;

        public override void onFixedUpdate()
        {
        }

        public override void OnStepStart()
        {
            Controller.events.onTeleportInvoke();
            Controller.Teleport();
            ChangeState(PortalStates.Idle);
        }

        public override void OnTriggerEnter(Collider other)
        {
        }
    }
}