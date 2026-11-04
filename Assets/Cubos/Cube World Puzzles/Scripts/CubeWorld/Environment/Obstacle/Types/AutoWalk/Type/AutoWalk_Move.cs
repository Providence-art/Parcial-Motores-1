using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class AutoWalk_Move : IAutoWalkBase
    {
        public AutoWalk_Move(AutoWalk controller) : base(AutoWalkStates.Move, controller) { }
        private Vector3 targetPosition;
        private Vector3 Position => Controller.transform.position;
        public override void ObjectIsStanding(GameObject obj)
        {
        }

        public override void onEnd()
        {
            if(Buffer.Hero)Buffer.Hero.AutowalkIsOver();
        }

        public override void onStart()
        {
            Reset();
            if (!Controller.getTargetPosition(out targetPosition))
            {
                ChangeState(AutoWalkStates.Idle);
                return;
            }
            if (Buffer.Hero) Buffer.Hero.PreparateToAutowalk();
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
            Events.onTravelingStartedInvoke();
        }

        public override void onUpdate()
        {
            if (isLock) return;
            positionFrequency();
            rotationFrequency();
            endPointCheck();
        }
        protected void positionFrequency()
        {
            Controller.transform.position = Vector3.MoveTowards(Position, targetPosition, Settings.Speed * Time.deltaTime);
            if (Controller.buffer.Hero) Controller.buffer.Hero.UpdateHeroPositionFromObstacle(Controller.obstacle);
        }
        protected void rotationFrequency()
        {

        }
        protected void endPointCheck()
        {
            if (Controller.transform.position != targetPosition) return;
            Controller.buffer.changeTowardsSide();
            Controller.buffer.travelCount++;
            isLock = true;
            Events.onTravelingEndedInvoke();
            ChangeState(AutoWalkStates.Unloading);
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void onFixUpdate()
        {
        }
    }
}