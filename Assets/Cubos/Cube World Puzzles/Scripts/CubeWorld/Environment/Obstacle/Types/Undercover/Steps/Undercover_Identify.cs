using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Undercover_Identify : IUndercover
    {
        public Undercover_Identify(Undercover controller) => Controller = controller;

        public UndercoverStates Steps => UndercoverStates.Identify;

        public Undercover Controller { get; }

        public void ObjectIsStanding(GameObject obj)
        {
        }

        public void OnStepFinish()
        {
        }

        public void OnStepStart()
        {
            Controller.obstacle.DisplayeAllFaces(true);
            Controller.events.onBeingIdentifiedInvoke();
        }
    }
}