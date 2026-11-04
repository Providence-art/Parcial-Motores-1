namespace HashGame.CubeWorld.OptimizedCube
{
    public class DoorState_Closing : DoorState_Movement
    {
        public DoorState_Closing(Door controller) : base(DoorState.Closing, controller) { }
        public override void onFinish()
        {
            Controller.buffer.Status = DoorStatus.Close;
        }
        public override void onStart()
        {
            if (Controller.terrainManager != null) Controller.terrainManager.PlaySFX(State);
            Reset();
            Controller.DoorDisplaye(true);
            events.onDoorCloseStarttInvoke();
        }
    }
}