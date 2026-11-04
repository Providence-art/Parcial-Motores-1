namespace HashGame.CubeWorld.HeroManager
{
    [System.Serializable]
    public class IHeroStep_Sliding : IHeroStep_Movement
    {

        public IHeroStep_Sliding(HeroController controller) : base(controller, HeroSteps.Sliding) { }
        #region variable
        protected HeroSteps lastStep;
        #endregion
        #region functions

        public override void OnStepStart()
        {
            Reset();
            lastStep = Controller.movementSteps.Last;
            if (!IHeroStep_Movement.MovementAxesTable.ContainsKey(lastStep))
            {
                HeroStep_Change(HeroSteps.Idle);
                return;
            }
            movementAxis = (IHeroStep_Movement.MovementAxis)IHeroStep_Movement.MovementAxesTable[lastStep];
            checkMoveTowardsToObstacle(); 
            AudioClipPlaySFX();
        }

        public override void OnUpdate()
        {
            if (isLock) return;
            positionFrequency();
            endPointCheck();
        }
        #endregion
    }
}