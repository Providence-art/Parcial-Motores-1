using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Undercover_Init : IUndercover
    {
        public Undercover_Init(Undercover controller) => Controller = controller;

        public UndercoverStates Steps => UndercoverStates.Init;

        public Undercover Controller { get; }

        public void ObjectIsStanding(GameObject obj)
        {
        }

        public void OnStepFinish()
        {
        }

        public void OnStepStart()=>Controller.ChangeState(UndercoverStates.Stealth);
    }
}