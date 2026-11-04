namespace HashGame.CubeWorld.HeroManager
{
    public enum HeroSteps : int
    {
        Init,
        Idle,
        GoForward,
        GoBack,
        GoRight,
        GoLeft,
        GoUpForward,
        GoUpBack,
        GoUpRight,
        GoUpLeft,
        Sliding,
        Lock,
        Falling,
        Destroy
    }
    public static class HeroStepsExtentions
    {
        public static bool IsStable(this HeroSteps step)
        {
            switch (step)
            {
                case HeroSteps.Falling: return false;
                default: return true;
            }
        }
        public static HeroSteps ToUpAndMoving(this HeroSteps step)
        {
            switch (step)
            {
                case HeroSteps.GoForward: return HeroSteps.GoUpForward;
                case HeroSteps.GoBack: return HeroSteps.GoUpBack;
                case HeroSteps.GoRight: return HeroSteps.GoUpRight;
                case HeroSteps.GoLeft: return HeroSteps.GoUpLeft;
            }
            return default(HeroSteps);
        }
        public static HeroSteps ToDirectMovement(this HeroSteps step)
        {
            switch (step)
            {
                case HeroSteps.GoUpForward:
                case HeroSteps.GoForward:
                    return HeroSteps.GoForward;
                case HeroSteps.GoBack:
                case HeroSteps.GoUpBack:
                    return HeroSteps.GoBack;
                case HeroSteps.GoLeft:
                case HeroSteps.GoUpLeft:
                    return HeroSteps.GoLeft;
                case HeroSteps.GoUpRight:
                case HeroSteps.GoRight:
                    return HeroSteps.GoRight;
            }
            return default(HeroSteps);
        }
        public static bool IsDirectMovement(this HeroSteps step)
        {
            switch (step)
            {
                case HeroSteps.GoForward:
                case HeroSteps.GoBack:
                case HeroSteps.GoRight:
                case HeroSteps.GoLeft:
                    return true;
            }
            return false;
        }
        public static bool IsMovementStep(this HeroSteps step)
        {
            switch (step)
            {
                case HeroSteps.GoForward:
                case HeroSteps.GoBack:
                case HeroSteps.GoRight:
                case HeroSteps.GoLeft:
                case HeroSteps.GoUpForward:
                case HeroSteps.GoUpBack:
                case HeroSteps.GoUpRight:
                case HeroSteps.GoUpLeft:
                    return true;
            }
            return false;
        }
        public static int ToIndex(this HeroSteps step) => (int)step;
    }
    public interface IHeroStep
    {
        public HeroSteps Step { get; }
        public HeroController Controller { get; }
    }
}