using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class IPortalBase : IPortal
    {
        public IPortalBase(PortalStates state, Portal controller)
        {
            this.Controller = controller;
            this.States = state;
        }
        public PortalStates States { get; }
        public Portal Controller { get; }
        public abstract void OnStepStart();
        public abstract void onFixedUpdate();
        public abstract void OnTriggerEnter(Collider other);
        public abstract bool CanHostReception(GameObject obj);
        protected void ChangeState(PortalStates state)
        {
            Controller.ChangeState(state);
        }
    }
}