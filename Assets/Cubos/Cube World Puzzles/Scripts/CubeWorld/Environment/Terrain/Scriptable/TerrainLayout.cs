using UnityEngine;
namespace HashGame.CubeWorld.TerrainConstruction
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Unity Editor/" + NAME)]
    public class TerrainLayout : ScriptableObject
    {
        public const string NAME = "TerrainLayout";
        #region Instance
        private static TerrainLayout _instance;
        public static TerrainLayout Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<TerrainLayout>("Settings/"+ NAME)) == null)
                        _instance = new TerrainLayout();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public Material Obstacle;
        public Material IntegrationObstacle;
        public Material Door;
        public Material UndercoverObstacle;
        public Material SlideObstacle;
        public Material Checkpoint_Uncheck;
        public Material Checkpoint_Check;
        public Material Portal;
        public Material Destructible;
        public Material PortalDestination;
        public Material AutoWalk;
        public Material AutoWalkDestination;
        public Material Spawner;
        public Material SpawnerButton;
        public Material DoorLever;
        public Material GoalHome;
        #endregion
    }
}