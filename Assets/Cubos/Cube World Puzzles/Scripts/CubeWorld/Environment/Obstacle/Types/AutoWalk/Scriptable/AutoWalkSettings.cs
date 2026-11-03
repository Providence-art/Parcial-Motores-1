using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
namespace HashGame.CubeWorld.HeroManager
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/"+ NAME)]
    public class AutoWalkSettings : ScriptableObject
    {
        public const string NAME = "AutoWalkSettings";
        #region Instance
        private static AutoWalkSettings _instance;
        public static AutoWalkSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<AutoWalkSettings>("Settings/"+ NAME)) == null)
                        _instance = new AutoWalkSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public AutoWalk.AutowalkMovementCommandMode MovementCommandMode = AutoWalk.AutowalkMovementCommandMode.OnHeroDetection;
        public AutoWalk.AutowalkLoadHeroType autowalkLoadHeroMode = AutoWalk.AutowalkLoadHeroType.Human;
        [Min(0)]
        public float idleTime = 5.0f;
        [Min(0)]
        public float Speed = 5.0f;
        public bool backToStartAfterUnloading = true;

        public LimitUnlimitEnum autowalkTravelCountMode = LimitUnlimitEnum.Unlimited;
        [Min(1)]
        public int MaxTravelCount = 1;
        #endregion
    }
}