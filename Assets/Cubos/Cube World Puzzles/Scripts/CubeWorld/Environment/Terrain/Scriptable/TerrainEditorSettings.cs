using UnityEngine;
namespace HashGame.CubeWorld.TerrainConstruction
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Unity Editor/" + NAME)]
    public class TerrainEditorSettings : ScriptableObject
    {
        public const string NAME = "TerrainEditorSettings";
        #region Instance
        private static TerrainEditorSettings _instance;
        public static TerrainEditorSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<TerrainEditorSettings>("Settings/" + NAME)) == null)
                        _instance = new TerrainEditorSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public Color SelectedObstacleColor = Color.green;
        public Color MoseOverObstacleColor = Color.red;
        public Color SuggestionCubeColor = Color.green;
        public Color AddCubeLineColor = Color.blue;
        public Color SelectedWallColor = Color.black;
        #endregion
    }
}
