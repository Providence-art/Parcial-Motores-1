using UnityEngine;

namespace HashGame.CubeWorld.HeroManager
{
    public class IHeroStep_Falling : IHeroStep_Base
    {
        public IHeroStep_Falling(HeroController controller) : base(controller, HeroSteps.Falling) { }
        private bool timing = false;
        private const float T = 1.0f;
        private float t;
        private float endPosition;
        public override void OnStepStart()
        {
            Reset();
            TerrainManager terrainManager = GameObject.FindAnyObjectByType<TerrainManager>();
            timing = (terrainManager == null || terrainManager.terrainData.Min_Y == null);
            if (!timing) endPosition = terrainManager.terrainData.Min_Y.transform.position.y;
            Controller.rb.isKinematic = false;
            AudioClipPlaySFX(Step);
        }
        public override void OnUpdate()
        {
            if (isLock) return;
            if (timing)
            {
                t += Time.deltaTime;
                if (t > T)
                {
                    FallingEnded();
                }
            }
            else if (Controller.transform.position.y < endPosition)
            {
                FallingEnded();
            }
        }
        public override void OnStepFinish() { }
        protected override void Reset()
        {
            isLock = false;
            t = 0;
        }
        protected void FallingEnded()
        {
            Controller.rb.isKinematic = true;
            if (Controller.isRespawnable)
            {
                Controller.ResetHero();
                HeroStep_Change(HeroSteps.Idle);
                return;
            }
            HeroStep_Change(HeroSteps.Destroy);
        }
    }
}