using UnityEngine;
namespace HashGame.CubeWorld.OptimizedCube
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/" + NAME)]
    public class DestructibleSettings : ScriptableObject
    {
        public const string NAME = "DestructibleSettings";
        #region Instance
        private static DestructibleSettings _instance;
        public static DestructibleSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<DestructibleSettings>("Settings/"+ NAME)) == null)
                        _instance = new DestructibleSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public bool SwitchingBetweenColors = true;
        public Color colorDangerous = Color.black;
        public Color colorSafe = Color.white;
        #endregion
    }
}