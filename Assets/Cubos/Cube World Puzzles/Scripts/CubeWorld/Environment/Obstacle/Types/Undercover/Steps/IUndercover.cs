using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public enum UndercoverStates : int
    {
        Init = 0,
        Stealth,
        Identify
    }
    public interface IUndercover
    {
        public UndercoverStates Steps { get; }
        public Undercover Controller { get; }
        public void ObjectIsStanding(GameObject obj);
        public void OnStepFinish();
        public void OnStepStart();
    }
}