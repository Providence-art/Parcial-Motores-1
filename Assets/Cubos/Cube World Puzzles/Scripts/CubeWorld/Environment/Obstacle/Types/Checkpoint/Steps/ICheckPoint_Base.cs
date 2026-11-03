using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine;
namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class ICheckPoint_Base : ICheckpoint
    {
        public ICheckPoint_Base(CheckPointSteps step, CheckPoint controller)
        {
            Step = step;
            Controller = controller;
        }
        public CheckPointSteps Step { get; }
        public CheckPoint Controller { get; }
        public CheckPoint.CheckPointEventsStruct events => Controller.events;
        public abstract bool IsCheck { get; }
        public abstract void ObjectIsStanding(GameObject obj);
        public abstract void OnStepStart();
        public abstract void OnStepFinish();
        public TerrainLayout terrainLayout
        {
            get
            {
                if (Controller == null || Controller.obstacle.terrainManager == null || Controller.obstacle.terrainManager.terrainLayout == null)
                {
                    return TerrainLayout.Instance;
                }
                return Controller.obstacle.terrainManager.terrainLayout;
            }
        }
    }
}