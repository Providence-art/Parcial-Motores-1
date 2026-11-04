namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class DoorStateBase : IDoorState
    {
        public DoorStateBase(DoorState doorState, Door controller)
        {
            this.State = doorState;
            this.Controller = controller;
        }
        public DoorState State { get; }
        public Door Controller { get; }
        public Door.DoorEvents events=>Controller.events;
        public Door.DoorBuffer buffer => Controller.buffer;
        public Door.DoorSettings settings => Controller.settings;
        protected bool isLock = false;
        public abstract void onStart();
        public abstract void onFinish();
        public abstract void onUpdate();
        protected virtual void Reset()
        {
            isLock = false;
        }
        public void ChangeState(DoorState state, bool force = false)
        {
            isLock = true;
            Controller.ChangeState(state, force);
        }
    }
}
