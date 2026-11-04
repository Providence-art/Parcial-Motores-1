using UnityEngine;
using static HashGame.CubeWorld.OptimizedCube.DestructibleObstacle;

namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class IDestructibleObstacleBase : IDestructibleObstacle
    {
        public IDestructibleObstacleBase(DestructibleStates state, DestructibleObstacle controller)
        {
            State = state;
            Controller = controller;
        }
        public DestructibleLocalEvents events => Controller.events;
        public DestructibleStates State { get; }
        public DestructibleObstacle Controller { get; }
        public void ChangeState(DestructibleStates state,bool force=false) => Controller.ChangeState(state, force);
        public abstract void ObjectIsStanding(GameObject obj);
        public abstract void onFixUpdate();
        public abstract void onStart();
    }
}
