namespace HashGame.CubeWorld.HeroManager
{
    public class HeroStep_Idle : IHeroStep_Base
    {
        public HeroStep_Idle(HeroController controller) : base(controller, HeroSteps.Idle) { }

        public override void OnStepStart()
        {
            Reset();
            if (!CheckIsGrounded()) return;
            CheckGroundType();
        }
        public override void OnUpdate()
        {
            if (isLock || Controller.IsStateChangeLock) return;
            if (Controller.inputController.IsFlag(InputManager.Flags.Right))
            {
                HeroStep_Change(HeroSteps.GoRight);
            }
            else if (Controller.inputController.IsFlag(InputManager.Flags.Left))
            {
                HeroStep_Change(HeroSteps.GoLeft);
            }
            else if (Controller.inputController.IsFlag(InputManager.Flags.Forward))
            {
                HeroStep_Change(HeroSteps.GoForward);
            }
            else if (Controller.inputController.IsFlag(InputManager.Flags.Back))
            {
                HeroStep_Change(HeroSteps.GoBack);
            }
        }
        public override void OnStepFinish() { }
    }
}