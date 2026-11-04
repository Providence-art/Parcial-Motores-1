using System.Collections.Generic;
using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
namespace HashGame.CubeWorld
{
    [RequireComponent(typeof(TerrainData))]
    public class TerrainManager : MonoBehaviour
    {
        #region variables
        #region cubes
        public int CubesCount { get => cubesList.Count; }
        [HideInInspector] public List<GameObject> cubesList = new List<GameObject>();
        #endregion
        #region GoalHome
        [HideInInspector] public List<GoalHome> goalHomes = new List<GoalHome>();
        #endregion
        #region CheckPoints
        public CheckPoint CheckPointLead { get; protected set; }
        public List<CheckPoint> checkPointCubes = new List<CheckPoint>();
        #endregion
        public TerrainManagerEvent events = new TerrainManagerEvent();
        public TerrainManagerLocalSettings localSettings = new TerrainManagerLocalSettings()
        {
            useLayoutMaterials = true,
            limitedVisibility = false,
            visionRadius = 5f
        };
        public TerrainLayout terrainLayout;
        public TerrainLayout terrainMaterials
        {
            get
            {
                if (terrainLayout == null) return TerrainLayout.Instance;
                return terrainLayout;
            }
        }
        //
        [HideInInspector] public TerrainAudioSettings _audioSettings;
        public TerrainAudioSettings AudioSettings
        {
            get
            {
                if (_audioSettings == null)
                {
                    if ((_audioSettings = TerrainAudioSettings.Instance) == null)
                        _audioSettings = new TerrainAudioSettings();
                }
                return _audioSettings;
            }
        }
        #region editor
#if UNITY_EDITOR

#endif
        #endregion
        #region HideInInspector
        [HideInInspector] public TerrainData terrainData;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            terrainData = GetComponent<TerrainData>();
            if (terrainLayout == null)
            {
                terrainLayout = TerrainLayout.Instance;
            }
            if (_audioSettings == null)
            {
                _audioSettings = TerrainAudioSettings.Instance;
            }
        }
        private void Awake()
        {
            AudioManager audioManager = FindAnyObjectByType<AudioManager>();
            if (audioManager == null)
            {
                if (BasicExtensions.Camera) BasicExtensions.Camera.AddComponent<AudioManager>();
                else this.AddComponent<AudioManager>();
            }
        }
        #endregion
        #region checkPointCubes
        public void checkPointCubes_FlagRaising(CheckPoint leader)
        {
            CheckPointLead = leader;
            //if (leader == null) return;
            for (int i = 0; i < checkPointCubes.Count; i++)
            {
                CheckPoint checkPoint = checkPointCubes[i];
                if (checkPoint == null || checkPoint.obstacle.obstacleType != ObstacleType.Checkpoint)
                {
                    checkPointCubes.RemoveAt(i--);
                    continue;
                }
                if (checkPoint == leader) continue;
                checkPoint.ChangeState(CheckPointSteps.Uncheck);
            }
        }
        public void checkPointCubes_Add(CheckPoint node)
        {
            if (node == null || node.obstacle.obstacleType != ObstacleType.Checkpoint) return;
            if (checkPointCubes.Contains(node)) return;
            checkPointCubes.Add(node);
        }
        public void checkPointCubes_Remove(CheckPoint obstacle)
        {
            if (obstacle == null) return;
            checkPointCubes.Remove(obstacle);
        }
        public void checkPointCubesClear() => checkPointCubes.Clear();
        public void checkPointCubes_Clean()
        {
            for (int i = 0; i < checkPointCubes.Count; i++)
            {
                if (checkPointCubes[i] == null || checkPointCubes[i].obstacleType != ObstacleType.Checkpoint)
                {
                    checkPointCubes.RemoveAt(i--);
                }
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        #endregion
        public void Cleaning()
        {
            checkPointCubes_Clean();
            cubesList_Clean();
            GoalHome_Clean();
        }
        #region cubes
        public void Cubes_Check(GameObject obj)
        {
            if (obj == null) return;
            if (cubesList.Contains(obj)) return;
            cubesList_AddCube(obj);
        }
        protected void cubesList_AddCube(GameObject node)
        {
            cubesList_Clean();
            if (node == null) return;
            cubesList.Add(node);
            terrainData.CheckCube(node);
        }
        protected void cubesList_RemoveCube(GameObject node)
        {
            if (node == null) return;
            cubesList.Remove(node);
            cubesList_Clean();
        }
        public void cubesList_Clean()
        {
            cubesList.RemoveAll(item => item == null);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        #endregion
        #region GoalHome
        public void GoalHome_Add(GoalHome node)
        {
            if (node == null) return;
            if (goalHomes.IndexOf(node) >= 0) return;
            goalHomes.Add(node);
        }
        public void GoalHome_Remove(GoalHome node)
        {
            goalHomes.Remove(node);
            GoalHome_Clean();
        }
        public void GoalHome_Clean()
        {
            goalHomes.RemoveAll(item => item == null);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        #endregion
        #region Create or Delete cube
        public GameObject CreateCube(GameObject selectedCube, CubeFace face)
        {
            Obstacle selectedCubeFunction;
            if (selectedCube == null || (selectedCubeFunction = selectedCube.GetComponent<Obstacle>()) == null)
            {
                return CreateCube();
            }
            if (selectedCubeFunction.neighborData.HasNeighbor(face) || !selectedCubeFunction.IsOptimizable)
            {
                return null;
            }
            Vector3 position = selectedCubeFunction.Position
                + Obstacle.getDirection(face) * (2 * selectedCubeFunction.getSize(face));
            GameObject node = CreateCube(position);
            if (node != null)
            {
                selectedCubeFunction.neighborData.SetNeighbor(face, node);
                var func = node.GetComponent<DynamicCubeNeighborData>();
                //selectedCubeFunction.neighborData.CheckNeighbors();
                func.CheckNeighbors();
            }
            return node;
        }
        public GameObject CreateCube() => CreateCube(Vector3.zero);
        public GameObject CreateCube(Vector3 position) => createCube<Obstacle>(position);
        protected GameObject createCube<T>(Vector3 position) where T : Obstacle
        {
            GameObject obj = new GameObject("Terrain Cube" + CubesCount + 1);
            obj.transform.parent = transform;
            obj.transform.position = position;
            T func = obj.AddComponent<T>();
            func.CreateCube(GetMaterial(func.obstacleType));
            func.terrainManager = this;
            cubesList_AddCube(obj);
            return obj;
        }
        public GameObject CreateDoor(List<GameObject> cubes)
        {
            GameObject obj = new GameObject(Door.NAME + " " + CubesCount + 1);
            obj.transform.parent = transform;
            Door door = obj.AddComponent<Door>();
            door.ConvertToDoor(cubes);
            door.terrainManager = this;
            door.SetMaterial(terrainLayout.Door);
            return obj;
        }
        public void OnDestroy_Cube(GameObject obj)
        {
            if (!obj) return;
            cubesList_RemoveCube(obj);
            cubesList_Clean();
        }
        public void CreateFirstCube()
        {
            CreateCube();
        }
        public void DeleteCube(GameObject obj)
        {
            cubesList_RemoveCube(obj);
            BasicExtensions.DestroyObject(obj);
        }
        public void DeleteCubes()
        {
            foreach (var node in cubesList)
            {
                BasicExtensions.DestroyObject(node);
            }
            cubesList.Clear();
            BasicExtensions.DeleteChildren(this.transform);
        }
        #endregion
        #region TerrainLand
        #region line generate
        public GameObject[] GenerateInLine(Obstacle lead, CubeFace face, int count = 1)
        {
            List<GameObject> result = new List<GameObject>();
            if (lead == null || count < 1) return result.ToArray();
            Obstacle current = lead;
            for (int i = 0; i < count; i++)
            {
                if (current == null) break;
                if (current.neighborData.HasNeighbor(face))
                {
                    current = (Obstacle)current.neighborData.GetNeighbor<Obstacle>(face);
                    continue;
                }
                var node = CreateCube(current.gameObject, face);
                result.Add(node);
                current = node.GetComponent<Obstacle>();
            }
            return result.ToArray();
        }
        #endregion
        #region Quadrilateral
        public GameObject[] TerrainLand_GenerateQuadrilateral(TerrainLand_Quadrilateral pattern, bool autoIntegration = false)
        {
            List<GameObject> result = new List<GameObject>();
            if (pattern == null) return result.ToArray();
            DeleteCubes();
            for (int i = 0; i < pattern.width; i++)
            {
                for (int j = 0; j < pattern.length; j++)
                {
                    var node = pattern.GetData(i, j);
                    if (node == null || node.Length == 0) continue;
                    Vector3 startPosition = 2 * i * Vector3.forward * Obstacle.SIZE
                        + 2 * j * Vector3.right * Obstacle.SIZE;
                    Obstacle autoIntegrationNode = default(Obstacle);
                    for (int k = 0; k < node[0].Count; k++)
                    {
                        int dir = 1;
                        if (node[0].Direction == TerrainLand.SurfaceDataDirection.Down) dir = -1;
                        Vector3 newPos = startPosition + k * dir * Vector3.up * (2 * Obstacle.SIZE);
                        var item = createCube<Obstacle>(newPos);
                        if (node != null)
                        {
                            result.Add(item);
                        }
                        if (k == 0 && autoIntegration) autoIntegrationNode = item.GetComponent<Obstacle>();
                    }
                    if (autoIntegration && autoIntegrationNode != null)
                    {
                        autoIntegrationNode.IntegrationDownUp();
                    }
                }
            }
            return result.ToArray();
        }
        public GameObject[] TerrainLand_GenerateQuadrilateral(TerrainLand_Quadrilateral pattern, Obstacle lead, bool autoIntegration = false)
        {
            List<GameObject> result = new List<GameObject>();
            if (pattern == null) return result.ToArray();
            if (lead == null)
            {
                return TerrainLand_GenerateQuadrilateral(pattern, autoIntegration);
            }
            //
            int[] cubeFacesIndexArray = new int[System.Enum.GetValues(typeof(CubeFace)).Length];
            for (int i = 0; i < cubeFacesIndexArray.Length; i++) cubeFacesIndexArray[i] = i;
            cubeFacesIndexArray.ToShuffleArray();
            //
            Vector3 lastPosition = lead.Position;
            Vector3 position = Vector3.zero;
            for (int i = 0; i < cubeFacesIndexArray.Length; i++)
            {
                CubeFace face = (CubeFace)cubeFacesIndexArray[i];
                if (lead.neighborData.HasNeighbor(face)) continue;
                position = lead.Position + Obstacle.getDirection(face) * (2 * lead.getSize(face));
            }
            for (int i = 0; i < pattern.width; i++)
            {
                for (int j = 0; j < pattern.length; j++)
                {
                    var node = pattern.GetData(i, j);
                    if (node == null || node.Length == 0) continue;
                    Vector3 startPosition = position + 2 * i * Vector3.forward * Obstacle.SIZE + 2 * j * Vector3.right * Obstacle.SIZE;
                    Obstacle autoIntegrationNode = default(Obstacle);
                    for (int k = 0; k < node[0].Count; k++)
                    {
                        int dir = 1;
                        if (node[0].Direction == TerrainLand.SurfaceDataDirection.Down) dir = -1;
                        Vector3 newPos = startPosition + k * dir * Vector3.up * (2 * Obstacle.SIZE);

                        Vector3 direction = Vector3.Normalize(newPos - lastPosition);

                        if (Physics.Raycast(lastPosition, direction, Vector3.Distance(lastPosition, newPos)))
                        {
                            lastPosition = newPos;
                            continue;
                        }
                        lastPosition = newPos;
                        var item = createCube<Obstacle>(newPos);
                        if (node != null)
                        {
                            result.Add(item);
                        }
                        if (k == 0 && autoIntegration) autoIntegrationNode = item.GetComponent<Obstacle>();
                    }
                    if (autoIntegration && autoIntegrationNode != null)
                    {
                        autoIntegrationNode.IntegrationDownUp();
                    }
                }
            }
            return result.ToArray();
        }
        #endregion
        #region snake generate
        public GameObject[] GenerateSnakeLine(Obstacle lead, SnakeLineData data)
        {
            List<GameObject> result = new List<GameObject>();
            if (lead == null || data.count < 1) return result.ToArray();
            //
            CubeFace[] facesAround = new CubeFace[] { CubeFace.Forward, CubeFace.Right, CubeFace.Back, CubeFace.Left, };
            facesAround.ToShuffleArray();
            CubeFace[] facesUpDown = new CubeFace[] { CubeFace.Up, CubeFace.Down };
            facesUpDown.ToShuffleArray();
            //
            Obstacle last = lead;
            int index = 0;
            int maxPenalty = data.count / 10;
            int penalty = 0;
            while (index++ < data.count)
            {
                CubeFace face;
                if (Random.Range(SnakeLineData.MIN_Coefficient, SnakeLineData.MAX_Coefficient) > data.heightRate)
                {
                    face = facesAround[Random.Range(0, facesAround.Length)];
                }
                else
                {
                    face = facesUpDown[Random.Range(0, facesUpDown.Length)];
                }
                if (last.neighborData.HasNeighbor(face))
                {
                    int i = Random.Range(0, result.Count);
                    if (i >= result.Count) break;
                    last = result[i].GetComponent<Obstacle>();
                    if (penalty++ > maxPenalty) break;
                    index--;
                    continue;
                }
                penalty = 0;
                Vector3 position = last.Position + face.ToVector3() * (2 * Obstacle.SIZE);
                result.Add(createCube<Obstacle>(position));
            }
            return result.ToArray();
        }
        #region struct
        [System.Serializable]
        public struct SnakeLineData
        {
            public const float MAX_Coefficient = 1.0f;
            public const float MIN_Coefficient = 0.0f;
            [Min(1)] public int count;
            [UnityEngine.Range(MIN_Coefficient, MAX_Coefficient)] public float heightRate;
        }
        #endregion
        #endregion
        #region Random walk
        public GameObject[] GenerateRandomWalk(RandomWalkStruct data, Obstacle lead = null)
        {
            List<GameObject> result = new List<GameObject>();
            if (data.height < 1 || data.width < 1) return result.ToArray();
            bool[,] map = new bool[data.width, data.height];

            int x = data.width / 2;
            int y = data.height / 2;

            for (int i = 0; i < data.steps; i++)
            {
                int direction = Random.Range(0, 4);
                switch (direction)
                {
                    case 0: // up
                        if (y < data.height - 1) y++;
                        break;
                    case 1: // down
                        if (y > 0) y--;
                        break;
                    case 2: // left
                        if (x > 0) x--;
                        break;
                    case 3: // right
                        if (x < data.width - 1) x++;
                        break;
                }
                map[x, y] = true;
                if (Random.Range(0, 100) < 10)
                {
                    int roomWidth = Random.Range(3, 6);
                    int roomHeight = Random.Range(3, 6);

                    for (int j = x - roomWidth / 2; j < x + roomWidth / 2; j++)
                    {
                        for (int k = y - roomHeight / 2; k < y + roomHeight / 2; k++)
                        {
                            if (j >= 0 && j < data.width && k >= 0 && k < data.height)
                            {
                                map[x, y] = true;
                            }
                        }
                    }
                }
            }
            Vector3 center = transform.position;
            if (lead) center = lead.Position
                    + Vector3.left * (2 * Obstacle.SIZE) * data.width / 2
                    + Vector3.back * (2 * Obstacle.SIZE) * data.height / 2;
            Vector3 lastPosition = center + Vector3.left * (2 * Obstacle.SIZE);
            for (x = 0; x < data.width; x++)
            {
                for (y = 0; y < data.height; y++)
                {
                    float _x = x * (2 * Obstacle.SIZE);
                    float _y = y * (2 * Obstacle.SIZE);
                    Vector3 position = Vector3.right * _x + Vector3.forward * _y + center;
                    Vector3 vector = position - lastPosition;
                    if (map[x, y])
                    {
                        if (!Physics.Raycast(lastPosition, vector.normalized, vector.magnitude))
                        {
                            result.Add(createCube<Obstacle>(position));
                        }
                    }
                    lastPosition = position;
                }
            }
            return result.ToArray();
        }
        #region Struct
        [System.Serializable]
        public struct RandomWalkStruct
        {
            public int width;
            public int height;
            public int steps;
            public int getMaxSteps() => width * height;
        }
        #endregion
        #endregion
        #region Dungeon 
        public GameObject[] DungeonGenerateAlgorithm(DungeonGenerator data, Obstacle lead = null)
        {
            List<GameObject> result = new List<GameObject>();
            if (data == null) return result.ToArray();
            if (!data.isGenerated) data.GenerateMap();
            //
            Vector3 center = transform.position;
            if (lead) center = lead.Position
                    + Vector3.left * (2 * Obstacle.SIZE) * data.Width / 2
                    + Vector3.back * (2 * Obstacle.SIZE) * data.Height / 2;
            Vector3 lastPosition = center + Vector3.left * (2 * Obstacle.SIZE);
            //
            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    float _x = x * (2 * Obstacle.SIZE);
                    float _y = y * (2 * Obstacle.SIZE);
                    Vector3 position = Vector3.right * _x + Vector3.forward * _y + center;
                    Vector3 vector = position - lastPosition;
                    if (data.Map[x, y])
                    {
                        if (!Physics.Raycast(lastPosition, vector.normalized, vector.magnitude))
                        {
                            result.Add(createCube<Obstacle>(position));
                        }
                    }
                    lastPosition = position;
                }
            }
            return result.ToArray();
        }
        #endregion
        #region Stairway
        public GameObject[] StairwayAlgorithm(StairwayStruct data, Obstacle lead = null)
        {
            List<GameObject> result = new List<GameObject>();
            if (!data.face.CanCreateSuggestionCubeFrame()) return result.ToArray();
            Vector3 startPosition = transform.position;
            if (lead) startPosition = lead.Position;
            int j = 0;
            for (int i = 0; i <= data.lenght; i++)
            {
                Vector3 position = startPosition + Obstacle.getDirection(data.face) * 2 * Obstacle.SIZE * i;
                if (data.groundConnection)
                {
                    for (int k = 0; k < j; k++)
                    {
                        Vector3 p = position + Obstacle.getDirection(CubeFace.Up) * 2 * Obstacle.SIZE * k;
                        result.Add(createCube<Obstacle>(p));
                    }
                }
                position += Obstacle.getDirection(CubeFace.Up) * 2 * Obstacle.SIZE * j++;
                var node = createCube<Obstacle>(position);
                result.Add(node);
                if (data.autoIntegration)
                {
                    Obstacle obstacle = node.GetComponent<Obstacle>();
                    if (obstacle) obstacle.IntegrationDownUp();
                }
            }

            return result.ToArray();
        }
        [System.Serializable]
        public struct StairwayStruct
        {
            [Min(1)]
            public int lenght;
            public CubeFace face;
            public bool autoIntegration;
            public bool groundConnection;
        }
        #endregion
        #endregion
        #region TerrainMaterials
        public Material GetMaterial(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Default:
                    if (localSettings.useLayoutMaterials) return terrainMaterials.Obstacle;
                    return localSettings.brushMaterial;
                case ObstacleType.Undercover: return terrainMaterials.UndercoverObstacle;
                case ObstacleType.Slide: return terrainMaterials.SlideObstacle;
                case ObstacleType.Checkpoint: return terrainMaterials.Checkpoint_Uncheck;
                case ObstacleType.Portal: return terrainMaterials.Portal;
                case ObstacleType.Destructible: return terrainMaterials.Destructible;
                case ObstacleType.PortalDestination: return terrainMaterials.PortalDestination;
                case ObstacleType.AutoWalk: return terrainMaterials.AutoWalk;
                case ObstacleType.AutoWalkDestination: return terrainMaterials.AutoWalkDestination;
                case ObstacleType.Spawner: return terrainMaterials.Spawner;
                case ObstacleType.SpawnerButton: return terrainMaterials.SpawnerButton;
                case ObstacleType.DoorLever: return terrainMaterials.DoorLever;
                case ObstacleType.GoalHome: return terrainMaterials.GoalHome;
            }
            return terrainMaterials.Obstacle;
        }
        public Material GetMaterial(Door door)
        {
            return terrainMaterials.Door;
        }
        #endregion
        #region AudioSettings
        public bool PlaySFX(ObstacleType type)
        {
            if (AudioSettings == null) return false;
            switch (type)
            {
                case ObstacleType.Undercover:
                    AudioManager.Instance.PlaySFX(AudioSettings.UndercoverIsDetected);
                    return true;
                case ObstacleType.Slide:
                    AudioManager.Instance.PlaySFX(AudioSettings.Slide);
                    return true;
                case ObstacleType.Checkpoint:
                    AudioManager.Instance.PlaySFX(AudioSettings.CheckPointIsCheck);
                    return true;
                case ObstacleType.Portal:
                    AudioManager.Instance.PlaySFX(AudioSettings.PortalActivate);
                    return true;
                case ObstacleType.GoalHome:
                    AudioManager.Instance.PlaySFX(AudioSettings.GoalHomeEnter);
                    return true;
            }
            return false;
        }
        public void PlaySFX(SpawnStates state)
        {
            if (AudioSettings == null) return;
            switch (state)
            {
                case SpawnStates.Generate:
                    AudioManager.Instance.PlaySFX(AudioSettings.SpawnerGenerate);
                    break;
                case SpawnStates.CoolDown:
                    AudioManager.Instance.PlaySFX(AudioSettings.SpawnerReady);
                    break;
            }
        }
        public void PlaySFX(DoorState state)
        {
            if (AudioSettings == null) return;
            AudioManager.Instance.PlaySFX(AudioSettings.DoorOpening);
        }

        public void PlaySFX(DestructibleStates state)
        {
            if (AudioSettings == null) return;
            switch (state)
            {
                case DestructibleStates.Destroy:
                    AudioManager.Instance.PlaySFX(AudioSettings.DestructibleDestoyed);
                    break;
                case DestructibleStates.Idle:
                    AudioManager.Instance.PlaySFX(AudioSettings.DestructibleWarning);
                    break;
            }
        }
        public void PlaySFX(AutoWalkStates state)
        {
            if (AudioSettings == null) return;
            switch (state)
            {
                case AutoWalkStates.Move:
                    AudioManager.Instance.PlaySFX(AudioSettings.AutowalkMoveStart);
                    break;
                case AutoWalkStates.Unloading:
                    AudioManager.Instance.PlaySFX(AudioSettings.AutowalkUnloading);
                    break;
            }
        }
        #endregion
        #region Game is over?
        public void CheckGoalHomes()
        {
            CheckWinning();
        }
        public bool CheckWinning()
        {
            if (goalHomes == null || goalHomes.Count == 0) return false;
            bool success = false;
            for (int i = 0; i < goalHomes.Count; i++)
            {
                if (goalHomes[i] == null)
                {
                    goalHomes.RemoveAt(i--);
                    continue;
                }
                if (!goalHomes[i].IsHeroHost()) return false;
                success = true;
            }
            if (success)
            {
                var heros = FindObjectsByType<HeroController>(FindObjectsSortMode.None);
                if (heros != null)
                {
                    foreach (var hero in heros)
                    {
                        if (hero == null) continue;
                        hero.isLock = true;
                    }
                }
#if UNITY_EDITOR
                Debug.Log("Game over.");
#endif
                events.onWinningInvoke();
            }
            return success;
        }
        #endregion
        #region extera
        public void reCalculate()
        {
            foreach (var node in cubesList)
            {
                if (node == null) continue;
                Obstacle obstacle = node.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    obstacle.UpdateMesh(Obstacle.ScaledVertices_Up(0));
                    obstacle.SetMaterial(GetMaterial(obstacle.obstacleType));
                    continue;
                }
            }
        }
        #endregion
        #region struct
        [System.Serializable]
        public class TerrainManagerEvent : StepsEventStructBase
        {
            public UnityEvent onWinning;
            public void onWinningInvoke() => invoke(ref onWinning);
        }
        [System.Serializable]
        public struct TerrainManagerLocalSettings
        {
            public bool limitedVisibility;
            public float visionRadius;
            public bool useLayoutMaterials;
            public Material brushMaterial;
        }
        #endregion
    }
}