using HashGame.CubeWorld.Extensions;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class CheckPoint : ObstacleTypesBase
    {
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.Checkpoint;
        [HideInInspector] public ICheckPoint_Base[] checkPointArray = new ICheckPoint_Base[System.Enum.GetValues(typeof(CheckPointSteps)).Length];
        public CheckPointSteps currentState { get; protected set; }
        public CheckPointEventsStruct events = new CheckPointEventsStruct() { CheckPhaseEvents = new StepsEventStruct2Phase(), UnCheckPhaseEvents = new StepsEventStruct2Phase() };
        #endregion
        #region Functions
        private void Awake()
        {
            checkPointArray[CheckPointSteps.Uncheck.ToIndex()] = new ICheckpoint_Uncheck(this);
            checkPointArray[CheckPointSteps.Check.ToIndex()] = new ICheckpoint_Check(this);
            ChangeState(CheckPointSteps.Uncheck, true);
        }
        #endregion
        #region functions
        public override void OnDestroyClass()
        {
        }
        public void ChangeState(CheckPointSteps state, bool force = false)
        {
            if (currentState == state && !force) return;
            checkPointArray[state.ToIndex()].OnStepFinish();
            currentState = state;
            checkPointArray[state.ToIndex()].OnStepStart();
        }
        public bool IsCheck()
        {
            return (obstacleType == ObstacleType.Checkpoint
                && checkPointArray != null
                && checkPointArray[currentState.ToIndex()] != null
                && checkPointArray[currentState.ToIndex()].IsCheck);
        }
        public override void ObjectIsStanding(GameObject obj)
        {
            checkPointArray[currentState.ToIndex()].ObjectIsStanding(obj);
            if (obstacle.terrainManager != null) obstacle.terrainManager.checkPointCubes_Add(this);
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct CheckPointEventsStruct
        {
            public StepsEventStruct2Phase CheckPhaseEvents;
            public StepsEventStruct2Phase UnCheckPhaseEvents;
        }
        #endregion
    }
}