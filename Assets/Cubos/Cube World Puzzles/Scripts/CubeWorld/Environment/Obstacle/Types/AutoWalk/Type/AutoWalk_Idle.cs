using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class AutoWalk_Idle : IAutoWalkBase
    {
        public AutoWalk_Idle(AutoWalk controller) : base(AutoWalkStates.Idle, controller) { }
        public override void ObjectIsStanding(GameObject obj)
        {
            if (obj == null || isOutOfService()) return;

            HeroController hero = obj.GetComponent<HeroController>();
            if (hero == null) return;
            switch (Controller.Settings.autowalkLoadHeroMode)
            {
                case AutoWalk.AutowalkLoadHeroType.Human:
                    if (!hero.IsHuman) return;
                    break;
                case AutoWalk.AutowalkLoadHeroType.Cpu:
                    if (hero.IsHuman) return;
                    break;
                default:
                    break;
            }
            if (!Controller.getTargetPosition(out var position)) return;
            Controller.buffer.Hero = hero;
            if (Settings.MovementCommandMode == AutoWalk.AutowalkMovementCommandMode.OnTiming)
            {
                return;
            }
            ChangeState(AutoWalkStates.Move);
        }

        public override void onEnd()
        {
        }

        public override void onStart()
        {
            Reset();
            Controller.buffer.Hero = null;
            if (Settings.backToStartAfterUnloading && !Buffer.isStayOnRespawnPosition())
            {
                //ChangeState(AutoWalkStates.Move);
            }
        }
        public override void OnTriggerExit(Collider other)
        {
        }

        public override void onUpdate()
        {
        }

        public override void onFixUpdate()
        {
            onTimingTravelFrequency();
            if (onTimingTravelFlag)
            {
                ChangeState(AutoWalkStates.Move);
            }
        }
    }
}