using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    [System.Serializable]
    public abstract class IAutoWalkBase : IAutoWalk
    {
        public IAutoWalkBase(AutoWalkStates state, AutoWalk controller)
        {
            this.Controller = controller;
            this.State = state;
        }
        #region variable
        protected bool isLock;
        public bool onTimingTravelFlag { get; protected set; }
        private float _t = 0.0f;
        public AutoWalkStates State { get; }
        public AutoWalk Controller { get; }
        public AutoWalk.AutoWalkEvents Events { get => Controller.events; }
        public AutoWalk.BufferStruct Data { get => Controller.buffer; }
        public AutoWalkSettings Settings { get => Controller.Settings; }
        public AutoWalk.BufferStruct Buffer { get => Controller.buffer; }
        #endregion
        public abstract void onStart();
        public abstract void onEnd();
        public abstract void onUpdate();
        public abstract void onFixUpdate();
        public abstract void ObjectIsStanding(GameObject obj);
        public abstract void OnTriggerExit(Collider other);
        protected virtual void Reset()
        {
            isLock = false;
            _t = 0.0f;
            onTimingTravelFlag = false;
        }
        public void ChangeState(AutoWalkStates state, bool force = false)
        {
            isLock = true;
            Controller.ChangeState(state, force);
        }
        public bool isOutOfService()
        {
            return (Settings.autowalkTravelCountMode == LimitUnlimitEnum.Limited
                && Buffer.travelCount >= Settings.MaxTravelCount);
        }
        protected void onTimingTravelFrequency()
        {
            if (onTimingTravelFlag || isLock || isOutOfService()) return;
            if (Settings.MovementCommandMode == AutoWalk.AutowalkMovementCommandMode.OnTiming)
            {
                if (_t >= Settings.idleTime)
                    onTimingTravelFlag = true;
                _t += Time.deltaTime;
            }
        }
    }
}