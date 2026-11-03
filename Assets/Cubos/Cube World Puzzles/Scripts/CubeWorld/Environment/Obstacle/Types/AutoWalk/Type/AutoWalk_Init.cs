using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class AutoWalk_Init : IAutoWalkBase
    {
        public AutoWalk_Init(AutoWalk controller) : base(AutoWalkStates.Init, controller) { }
        public override void ObjectIsStanding(GameObject obj)
        {
        }

        public override void onEnd()
        {
        }

        public override void onFixUpdate()
        {
        }

        public override void onStart()
        {
            Controller.transform.position = Buffer.StartPosition;
            Controller.buffer.inSpawnPosition = true;
            Controller.buffer.travelCount = 0;
            ChangeState(AutoWalkStates.Idle);
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void onUpdate()
        {
        }
    }
}