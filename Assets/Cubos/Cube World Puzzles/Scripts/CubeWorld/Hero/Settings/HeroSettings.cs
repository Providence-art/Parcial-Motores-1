using UnityEngine;
namespace HashGame.CubeWorld.HeroManager
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/Hero/" + NAME)]
    public class HeroSettings : ScriptableObject
    {
        public const string NAME = "HeroSettings";
        #region Instance
        private static HeroSettings _instance;
        public static HeroSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<HeroSettings>("Settings/" + NAME)) == null)
                        _instance = new HeroSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        [Min(0)]
        public float Speed = 5.0f;
        public bool EnableFalling = false;
        #endregion
    }
}