using UnityEngine;
namespace HashGame.CubeWorld.HeroManager
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/Hero/"+ NAME)]
    public class HeroAudioSettings : ScriptableObject
    {
        public const string NAME = "HeroAudioSettings";
        #region Instance
        private static HeroAudioSettings _instance;
        public static HeroAudioSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<HeroAudioSettings>("Settings/"+ NAME)) == null)
                        _instance = new HeroAudioSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public AudioClip FootStep;
        public AudioClip Falling;
        public AudioClip Destroying;
        public AudioClip Collecting;
        public AudioClip Trading;
        #endregion
    }
}