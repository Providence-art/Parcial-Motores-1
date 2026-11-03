using UnityEngine.InputSystem.XR;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DoorState_Opening : DoorState_Movement
    {
        public DoorState_Opening(Door controller) : base(DoorState.Opening, controller) { }
        public override void onFinish()
        {
            Controller.DoorDisplaye(false);
            Controller.buffer.Status=DoorStatus.Open;
            Controller.buffer.travelCount++;
        }
        public override void onStart()
        {
            Reset();
            if (Controller.terrainManager != null) Controller.terrainManager.PlaySFX(State);
            events.onDoorOpenStartInvoke();
        }
    }
}