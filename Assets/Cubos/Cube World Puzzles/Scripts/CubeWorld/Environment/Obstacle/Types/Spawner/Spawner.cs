using System.Collections.Generic;
using HashGame.CubeWorld.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Spawner : ObstacleTypesBase
    {
        #region 
        public SpawnerEvents events = new SpawnerEvents();
        public SpawnerBuffer buffer = new SpawnerBuffer();
        public SpawnerSettings _settings;
        public SpawnerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    if ((_settings = SpawnerSettings.Instance) == null) _settings = new SpawnerSettings();
                }
                return _settings;
            }
        }
        #region spawn
        public SpawnStates currentState { get; protected set; }
        public override ObstacleType MyObstacleType => ObstacleType.Spawner;
        [HideInInspector] public ISpawnerBase[] spawnerArray;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            if (!_settings) _settings = SpawnerSettings.Instance;
        }
        private void Start()
        {
            obstacle.boxCollider.size *= .9f;
            initState();
        }
        private void FixedUpdate()
        {
            if (currentState == SpawnStates.CoolDown)
            {
                ((Spawner_CoolDown)spawnerArray[(int)currentState]).onFixUpdate();
            }
        }
        #endregion
        #region functions
        public void TryGenerate() => spawnerArray[(int)currentState].Generate();
        public bool CanGenerate()
        {
            if (Settings.HeroPrefabs == null || Settings.HeroPrefabs.Length < 1) return false;
            if (Settings.GenerateType == LimitUnlimitEnum.Limited)
            {
                if (buffer.GenerateCount >= Settings.MaxGenerateCount) return false;
            }
            for (int i = 0; i < buffer.objects.Count; i++)
            {
                if (buffer.objects[i] == null)
                {
                    buffer.objects.RemoveAt(i--);
                    continue;
                }
            }
            if (buffer.objects.Count >= Settings.MaxObjectGenerateAtOnce) return false;
            if (IsSomeoneAbove()) return false;
            return true;
        }
        public bool IsSomeoneAbove() => obstacle.neighborData.Radiation(CubeFace.Up, out var hit);
        #region State
        private void initState()
        {
            spawnerArray = new ISpawnerBase[System.Enum.GetValues(typeof(SpawnStates)).Length];
            spawnerArray[(int)SpawnStates.Idle] = new Spawner_Idle(this);
            spawnerArray[(int)SpawnStates.Generate] = new Spawner_Generate(this);
            spawnerArray[(int)SpawnStates.CoolDown] = new Spawner_CoolDown(this);
            ChangeState(SpawnStates.Idle, true);
        }
        public void ChangeState(SpawnStates state, bool force = false)
        {
            if (currentState == state && !force) return;
            spawnerArray[(int)currentState].OnStepFinish();
            spawnerArray[(int)(currentState = state)].OnStepStart();
        }
        public override void ObjectIsStanding(GameObject obj)
        {
        }

        public override void OnDestroyClass()
        {
        }

        #endregion
        #endregion
        #region struct
        [System.Serializable]
        public class SpawnerBuffer
        {
            public int GenerateCount;
            [HideInInspector] public List<GameObject> objects = new List<GameObject>();
            [HideInInspector] public int objectsIndex;
        }
        [System.Serializable]
        public class SpawnerEvents : StepsEventStructBase
        {
            public UnityEvent onGenerate;
            public void onGenerateInvoke() => invoke(ref onGenerate);
        }
        #endregion
    }
}