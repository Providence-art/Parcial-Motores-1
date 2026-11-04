using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace HashGame.CubeWorld.TerrainConstruction
{
    public class IKeyStep_Idle : IKeyStep
    {
        public IKeyStep_Idle(KeyController controller) { Controller = controller; }
        public KeySteps Step => KeySteps.Idle;
        public KeyController Controller { get; }
        public KeyController.CollectableData data { get => Controller.keyData; }
        private bool isLock;

        public void OnStepFinish()
        {
        }

        public void OnStepStart()
        {
            isLock = false;
            if (!Controller.StayOnGround())
            {
                isLock = true;
                Controller.KeyStepChange(KeySteps.Destroy, true);
            }
        }

        public void OnUpdate()
        {
            if (isLock) return;
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other == null) return;
            HeroController hero = other.GetComponent<HeroController>();
            if (hero == null) return;
            if (!hero.IsHuman) return;
            if (!hero.IsCollecting(Controller.gameObject)) return;
            Controller.setOwner(hero);
            Controller.KeyStepChange(KeySteps.Follower);
        }
    }
}