using UnityEngine;
namespace HashGame.CubeWorld.OptimizedCube
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/"+ NAME)]
    public class SpawnerSettings : ScriptableObject
    {
        public const string NAME = "SpawnerSettings";
        #region Instance
        private static SpawnerSettings _instance;
        public static SpawnerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<SpawnerSettings>("Settings/"+ NAME)) == null)
                        _instance = new SpawnerSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public LimitUnlimitEnum GenerateType = LimitUnlimitEnum.Unlimited;
        [Min(0)] public int MaxGenerateCount;
        [Min(1)] public int MaxObjectGenerateAtOnce = 1;
        //
        public GameObject[] HeroPrefabs;
        public bool PrefabAutoConfig = true;
        public RandomInOrderEnum PrefabOrderType = RandomInOrderEnum.Random;
        //
        [Min(1)]
        public int coolDownTime = 5;
        public bool useColors = true;
        public Color onReadyColor = Color.green;
        public Color onCoolDownColor = Color.red;
        #endregion
# if UNITY_EDITOR
        public void LoadPrefabs() => PrefabManager.LoadHeroPrefabs();
#endif
    }
}