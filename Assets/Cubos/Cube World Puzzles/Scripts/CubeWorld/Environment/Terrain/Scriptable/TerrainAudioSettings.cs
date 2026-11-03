using UnityEngine;
namespace HashGame.CubeWorld.TerrainConstruction
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/"+ NAME)]
    public class TerrainAudioSettings : ScriptableObject
    {
        public const string NAME = "TerrainAudioSettings";
        #region Instance
        private static TerrainAudioSettings _instance;
        public static TerrainAudioSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<TerrainAudioSettings>("Settings/"+ NAME)) == null)
                        _instance = new TerrainAudioSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public AudioClip UndercoverIsDetected;
        public AudioClip Slide;
        public AudioClip CheckPointIsCheck;
        public AudioClip PortalActivate;
        public AudioClip DestructibleDestoyed;
        public AudioClip DestructibleWarning;
        public AudioClip AutowalkMoveStart;
        public AudioClip AutowalkUnloading;
        public AudioClip SpawnerGenerate;
        public AudioClip SpawnerReady;
        public AudioClip GoalHomeEnter;
        public AudioClip DoorOpening;
        #endregion
    }
}