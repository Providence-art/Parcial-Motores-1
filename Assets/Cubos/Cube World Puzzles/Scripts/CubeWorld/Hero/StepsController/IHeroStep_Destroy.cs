using HashGame.CubeWorld.Extensions;

namespace HashGame.CubeWorld.HeroManager
{
    [System.Serializable]
    public class IHeroStep_Destroy : IHeroStep_Base
    {

        public IHeroStep_Destroy(HeroController controller) : base(controller, HeroSteps.Destroy) { }
        #region variable
        #endregion
        #region functions
        public override void OnStepStart()
        {
            AudioClipPlaySFX();
            events.onHeroDeadInvoke();
            if (!Controller.isRespawnable)
            {
                BasicExtensions.DestroyObject(Controller.gameObject);
            }
        }

        public override void OnUpdate()
        {
        }

        public override void OnStepFinish()
        {
        }
        #endregion
    }
}