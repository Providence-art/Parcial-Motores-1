using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class ICheckpoint_Check : ICheckPoint_Base
    {
        public ICheckpoint_Check(CheckPoint controller) : base(CheckPointSteps.Check, controller) { }
        public override bool IsCheck => true;
        public override void ObjectIsStanding(GameObject obj)
        {
        }
        public override void OnStepFinish()
        {
            events.UnCheckPhaseEvents.onPhaseEnd_Invoke();
        }
        public override void OnStepStart()
        {
            if (Controller.obstacle.terrainManager != null)
            {
                Controller.obstacle.terrainManager.checkPointCubes_FlagRaising(Controller);
            }
            Controller.obstacle.SetMaterial(terrainLayout.Checkpoint_Check);
            events.UnCheckPhaseEvents.onPhaseStart_Invoke();
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null)
                Controller.obstacle.terrainManager.PlaySFX(Controller.MyObstacleType);
        }
    }
}