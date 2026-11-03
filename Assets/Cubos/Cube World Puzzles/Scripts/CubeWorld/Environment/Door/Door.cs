using System.Collections.Generic;
using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    #region enum
    public enum DoorType : int
    {
        None = 0,
        LeverOpen,
        keyOpen
    }
    public enum DoorStatus : int
    {
        Open,
        Close
    }
    #endregion
    public class Door : DynamicCubeFaceRendering
    {
        #region variable
        public const string NAME = "Door";
        [HideInInspector] public DoorType doorType;
        #region door type base
        [HideInInspector] public DoorTypeBase doorTypeBase;
        public KeyOpenDoor keyOpenDoor
        {
            get
            {
                if (doorTypeBase.doorType == DoorType.keyOpen) return (KeyOpenDoor)doorTypeBase;
                return null;
            }
        }
        public LeverOpenDoor leverOpenDoor
        {
            get
            {
                if (doorTypeBase.doorType == DoorType.LeverOpen) return (LeverOpenDoor)doorTypeBase;
                return null;
            }
        }
        #endregion
        [HideInInspector]
        public DoorBuffer buffer = new DoorBuffer()
        {
            Status = DoorStatus.Close,
        };
        public DoorSettings settings = new DoorSettings()
        {
            Speed = 1.0f,
        };
        public DoorEvents events = new DoorEvents();
        #region hide
        [HideInInspector] public TerrainManager _terrainManager;
        public TerrainManager terrainManager
        {
            get
            {
                if (_terrainManager == null)
                {
                    _terrainManager = GameObject.FindAnyObjectByType<TerrainManager>();
                }
                return _terrainManager;
            }
            set { _terrainManager = value; }
        }
        #endregion
        #region DoorState
        public DoorState currentState { get; protected set; }
        [HideInInspector] public DoorStateBase[] doorStateArray;
        #endregion
        #endregion
        #region Functions
        private void Start()
        {
            initDoorStateBase();
            buffer.StartPosition = Position;
            buffer.EndPosition = Position + Vector3.down * RealSize.y;
            if (terrainManager && terrainManager.localSettings.limitedVisibility)
            {
                DisplayeAllFaces(false);
            }
        }
        private void Update()
        {
            doorStateArray[(int)currentState].onUpdate();
        }
        #endregion
        public void DoorDisplaye(bool enable)
        {
            boxCollider.enabled = enable;
            meshRenderer.enabled = enable;
        }
        #region DoorType
        public void SetDoorType(DoorType type, bool force = false)
        {
            if (doorType == type && !force) return;
            BasicExtensions.DestroyObject(doorTypeBase);
            doorType = type;
            switch (doorType)
            {
                case DoorType.LeverOpen:
                    doorTypeBase = gameObject.AddComponent<LeverOpenDoor>();
                    break;
                case DoorType.keyOpen:
                    doorTypeBase = gameObject.AddComponent<KeyOpenDoor>();
                    break;
            }
        }
        #endregion
        #region open close
        public void onLeverOpenGate()
        {
            if (doorType == DoorType.LeverOpen)
            {
                ChangeState(DoorState.Opening);
            }
        }
        public void onLeverCloseGate()
        {
            if (doorType == DoorType.LeverOpen)
            {
                ChangeState(DoorState.Closing);
            }
        }
        #endregion
        #region Key
        public bool NeedKey()
        {
            if (doorType != DoorType.keyOpen) return false;
            if (currentState != DoorState.Idle) return false;
            return keyOpenDoor != null && keyOpenDoor.NeedKey();
        }
        public void KeyIsOnWay()
        {
            if (keyOpenDoor != null) keyOpenDoor.KeyIsOnWay();
        }
        public void KeyRecive(CollectableObject obj)
        {
            if (keyOpenDoor != null) keyOpenDoor.KeyRecive(obj);
        }
        #endregion
        #region DoorStateBase
        private void initDoorStateBase()
        {
            buffer.Status = DoorStatus.Close;
            doorStateArray = new DoorStateBase[System.Enum.GetValues(typeof(DoorState)).Length];
            doorStateArray[(int)DoorState.Idle] = new DoorState_Idle(this);
            doorStateArray[(int)DoorState.Opening] = new DoorState_Opening(this);
            doorStateArray[(int)DoorState.Closing] = new DoorState_Closing(this);

            ChangeState(DoorState.Idle, true);
        }
        public void ChangeState(DoorState state, bool force = false)
        {
            if (currentState == state && !force) return;
            doorStateArray[(int)currentState].onFinish();
            doorStateArray[(int)(currentState = state)].onStart();
        }
        #endregion
        #region functions
        public bool ConvertToDoor(List<GameObject> cubes)
        {
            if (cubes == null || cubes.Count == 0) return false;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;
            Vector3 center = Vector3.zero;
            int cubesCount = 0;
            foreach (var cube in cubes)
            {
                Obstacle obstacle;
                if (cube == null
                    || (obstacle = cube.GetComponent<Obstacle>()) == null
                    || !obstacle.IsOptimizable) return false;

                Vector3 pos = cube.transform.position;
                center += pos;
                cubesCount++;
                if (pos.x < minX) minX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.z < minZ) minZ = pos.z;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y > maxY) maxY = pos.y;
                if (pos.z > maxZ) maxZ = pos.z;
            }
            center /= cubesCount;
            transform.position = center;
            // goto larger cube
            int sizeX = Mathf.RoundToInt(maxX - minX) + 1;
            int sizeY = Mathf.RoundToInt(maxY - minY) + 1;
            int sizeZ = Mathf.RoundToInt(maxZ - minZ) + 1;

            if (cubes.Count != sizeX * sizeY * sizeZ)
            {
                return false;
            }
            bool[,,] grid = new bool[sizeX, sizeY, sizeZ];
            foreach (var cube in cubes)
            {
                Vector3 pos = cube.transform.position;
                int x = Mathf.RoundToInt(pos.x - minX);
                int y = Mathf.RoundToInt(pos.y - minY);
                int z = Mathf.RoundToInt(pos.z - minZ);

                if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || z < 0 || z >= sizeZ || grid[x, y, z])
                {
                    return false;
                }
                grid[x, y, z] = true;
            }
            grid = null;
            //
            float scaleX = Mathf.Abs(maxX - minX) / 2;
            float scaleY = Mathf.Abs(maxY - minY) / 2;
            float scaleZ = Mathf.Abs(maxZ - minZ) / 2;
            CreateCube(Vertices(scaleX, scaleY, scaleZ));
            //UpdateMesh(Vertices(scaleX, scaleY, scaleZ));
            //
            DisplayeAllFaces(true);
            return true;
        }
        #endregion
        #region Static
        public static bool CanConvertToDoor(List<GameObject> cubes) => CheckIfFormLargeCube(cubes);
        public static bool CheckIfFormLargeCube(List<GameObject> cubes)
        {
            if (cubes == null || cubes.Count == 0) return false;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            foreach (var cube in cubes)
            {
                Obstacle obstacle;
                if (cube == null
                    || (obstacle = cube.GetComponent<Obstacle>()) == null
                    || !obstacle.IsOptimizable) return false;

                Vector3 pos = cube.transform.position;
                if (pos.x < minX) minX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.z < minZ) minZ = pos.z;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y > maxY) maxY = pos.y;
                if (pos.z > maxZ) maxZ = pos.z;
            }
            // goto larger cube
            int sizeX = Mathf.RoundToInt(maxX - minX) + 1;
            int sizeY = Mathf.RoundToInt(maxY - minY) + 1;
            int sizeZ = Mathf.RoundToInt(maxZ - minZ) + 1;

            if (cubes.Count != sizeX * sizeY * sizeZ)
            {
                return false;
            }
            bool[,,] grid = new bool[sizeX, sizeY, sizeZ];
            foreach (var cube in cubes)
            {
                Vector3 pos = cube.transform.position;
                int x = Mathf.RoundToInt(pos.x - minX);
                int y = Mathf.RoundToInt(pos.y - minY);
                int z = Mathf.RoundToInt(pos.z - minZ);

                if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || z < 0 || z >= sizeZ || grid[x, y, z])
                {
                    return false;
                }
                grid[x, y, z] = true;
            }
            return true;
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct DoorBuffer
        {
            public Vector3 StartPosition;
            public Vector3 EndPosition;
            public DoorStatus Status;
            public int travelCount;
            public void ToogleDoorStatus()
            {
                Status = (Status == DoorStatus.Close) ? DoorStatus.Open : DoorStatus.Close;
            }
        }
        [System.Serializable]
        public struct DoorSettings
        {
            [Min(1f)]
            public float Speed;
        }
        [System.Serializable]
        public class DoorEvents : StepsEventStructBase
        {
            public UnityEvent onDoorOpenStart;
            public UnityEvent onDoorCloseStart;
            public void onDoorOpenStartInvoke() => invoke(ref onDoorOpenStart);
            public void onDoorCloseStarttInvoke() => invoke(ref onDoorCloseStart);
        }
        #endregion
    }
}