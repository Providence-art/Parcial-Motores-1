using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class ICheckpoint_Uncheck : ICheckPoint_Base
    {
        public ICheckpoint_Uncheck(CheckPoint controller) : base(CheckPointSteps.Uncheck, controller) { }
        public override bool IsCheck => false;
        public override void ObjectIsStanding(GameObject obj)
        {
            if (obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (hero == null) return;
            if (!hero.IsHuman) return;
            Controller.ChangeState(CheckPointSteps.Check);
        }

        public override void OnStepFinish()
        {
            events.UnCheckPhaseEvents.onPhaseEnd_Invoke();
        }

        public override void OnStepStart()
        {
            Controller.obstacle.SetMaterial(terrainLayout.Checkpoint_Uncheck);
            events.UnCheckPhaseEvents.onPhaseStart_Invoke();
        }
    }
}