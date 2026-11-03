namespace HashGame.CubeWorld.OptimizedCube
{
    public class DoorState_Idle : DoorStateBase
    {
        public DoorState_Idle(Door controller) : base(DoorState.Idle, controller) { }
        public override void onFinish() { }
        public override void onStart() { }
        public override void onUpdate() { }
    }
}