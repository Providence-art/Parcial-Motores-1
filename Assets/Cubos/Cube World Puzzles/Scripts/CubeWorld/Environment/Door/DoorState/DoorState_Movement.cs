using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class DoorState_Movement : DoorStateBase
    {
        public DoorState_Movement(DoorState doorState, Door controller) : base(doorState, controller) { }
        public Vector3 targetPosition => (buffer.Status == DoorStatus.Close) ? buffer.EndPosition : buffer.StartPosition;
        public Vector3 Position => Controller.Position;
        public override void onFinish() { }
        public override void onStart() { }
        public override void onUpdate()
        {
            if (isLock) return;
            //Controller.transform.position = Vector3.SmoothDamp(Controller.transform.position, targetPosition, ref _velocity, settings.Speed);
            positionFrequency();
            rotationFrequency();
            endPointCheck();
        }
        protected void positionFrequency()
        {
            Controller.transform.position = Vector3.MoveTowards(Position, targetPosition, settings.Speed * Time.deltaTime);
        }
        protected void rotationFrequency() { }
        protected void endPointCheck()
        {
            if (Controller.transform.position != targetPosition) return;
            isLock = true;
            Controller.buffer.travelCount++;
            ChangeState(DoorState.Idle);
        }
    }
}