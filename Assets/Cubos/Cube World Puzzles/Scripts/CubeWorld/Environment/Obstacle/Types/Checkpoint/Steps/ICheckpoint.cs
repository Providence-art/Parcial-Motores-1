using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public enum CheckPointSteps : int
    {
        Uncheck = 0,
        Check
    }
    public static class CheckPointStepsExtentions
    {
        public static int ToIndex(this CheckPointSteps step) => (int)step;
    }
    public interface ICheckpoint
    {
        public CheckPointSteps Step { get; }
        public CheckPoint Controller { get; }
        public void ObjectIsStanding(GameObject obj);
        public void OnStepStart();
        public void OnStepFinish();
    }
}