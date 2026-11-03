using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Portal_Idle : IPortalBase
    {
        public Portal_Idle(Portal controller) : base(PortalStates.Idle, controller) { }

        public override bool CanHostReception(GameObject obj)
        {
            return (obj != null) && CanHostReception(obj.GetComponent<HeroController>());
        }
        private bool CanHostReception(HeroController hero)
        {
            if (hero == null || !Controller.CanHostReception()) return false;
            switch (Controller.settings.portalActiveHeroType)
            {
                case Portal.PortalActiveHeroType.Human: return hero.IsHuman;
                case Portal.PortalActiveHeroType.Cpu: return !hero.IsHuman;
                default: return true;
            }
        }
        public override void OnStepStart()
        {
        }
        public override void OnTriggerEnter(Collider other)
        {
            HeroController hero = other.GetComponent<HeroController>();
            if (!CanHostReception(hero)) return;
            hero.PreparateToTeleport();
            Controller.hero = hero;
            ChangeState(PortalStates.Active);
        }

        public override void onFixedUpdate()
        {
        }
    }
}