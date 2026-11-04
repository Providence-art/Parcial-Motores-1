namespace HashGame.CubeWorld.HeroManager
{
    public class IHeroStep_Init : IHeroStep_Base
    {
        public IHeroStep_Init(HeroController controller) : base(controller, HeroSteps.Init) { }

        public override void OnStepStart()
        {
            Reset();
            if (!CheckIsGrounded()) return;
            CheckGroundType();
            events.onHeroInitedInvoke();
            Controller.ChangeStep(HeroSteps.Idle);
        }
        public override void OnUpdate() { }
        public override void OnStepFinish() { }
    }
}