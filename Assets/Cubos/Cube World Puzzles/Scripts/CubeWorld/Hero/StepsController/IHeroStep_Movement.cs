using System.Collections;
using HashGame.CubeWorld.Informer;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine;

namespace HashGame.CubeWorld.HeroManager
{
    [System.Serializable]
    public class IHeroStep_Movement : IHeroStep_Base
    {
        #region variable
        #region const
        public const float RotationAngle = 360.0f / 4.0f;
        public static Hashtable MovementAxesTable = new Hashtable() {
            {HeroSteps.GoForward, new MovementAxis(Vector3.forward, Vector3.right) },
            {HeroSteps.GoBack, new MovementAxis(Vector3.back, Vector3.left)},
            {HeroSteps.GoLeft, new MovementAxis(Vector3.left, Vector3.forward)},
            {HeroSteps.GoRight, new MovementAxis(Vector3.right, Vector3.back)},
            {HeroSteps.GoUpForward, new MovementAxis(Vector3.up, Vector3.right)},
            {HeroSteps.GoUpBack, new MovementAxis(Vector3.up, Vector3.left)},
            {HeroSteps.GoUpRight, new MovementAxis(Vector3.up, Vector3.back)},
            {HeroSteps.GoUpLeft, new MovementAxis(Vector3.up, Vector3.forward)},
        };
        public static float getStepSize(HeroSteps step, HeroController controller)
        {
            switch (step)
            {
                case HeroSteps.GoForward:
                case HeroSteps.GoBack:
                    return controller.ForwardBackSize;
                case HeroSteps.GoRight:
                case HeroSteps.GoLeft:
                    return controller.RightLeftSize;
                case HeroSteps.GoUpForward:
                case HeroSteps.GoUpBack:
                case HeroSteps.GoUpLeft:
                case HeroSteps.GoUpRight:
                    return controller.UpDownSize;
            }
            return controller.ForwardBackSize;
        }
        #endregion
        public IHeroStep_Movement(HeroController controller, HeroSteps step) : base(controller, step)
        {
            if (MovementAxesTable.ContainsKey(step)) movementAxis = (MovementAxis)MovementAxesTable[step];
        }
        protected MovementData movementData;
        protected MovementAxis movementAxis;
        #region readonly
        public HeroSettings settings
        {
            get
            {
                if (Controller.Settings == null) return HeroSettings.Instance;
                return Controller.Settings;
            }
        }
        public float getStepSize() => getStepSize(Step, Controller);
        #endregion
        //
        protected float lastAngle;
        protected float distance;
        private bool _isMovementSuccessfully;
        #endregion
        public override void OnStepStart()
        {
            Reset();
            checkMoveTowardsToObstacle();
            if (_isMovementSuccessfully)
            {
                Controller.inputController.flagMappingUpdate(movementAxis.MotionAxis);
                if (Controller._camera) Controller._camera.ChangeRotation(movementAxis.MotionAxis);
                AudioClipPlaySFX();
                events.onHeroMovementInvoke();
            }
        }
        public override void OnStepFinish()
        {
            if (_isMovementSuccessfully) buffer.CurrentSteps++;
        }
        public override void OnUpdate()
        {
            if (isLock) return;
            positionFrequency();
            rotationFrequency();
            endPointCheck();
        }
        //
        #region functions
        protected void positionFrequency()
        {
            Controller.transform.position = Vector3.MoveTowards(Controller.Position, movementData.EndPoint, settings.Speed * Time.deltaTime);
        }
        protected void rotationFrequency()
        {
            float deltaX = Vector3.Distance(movementData.StartPoint, Controller.Position);
            float angle = RotationAngle * (deltaX / distance);
            float tetha = angle - lastAngle;
            if (Controller.CubeWheel)
                Controller.CubeWheel.transform.Rotate(((MovementAxis)MovementAxesTable[Step]).RotationAxis, tetha, Space.World);
            if (Controller.Head)
                Controller.Head.transform.forward = ((MovementAxis)MovementAxesTable[Step]).RotationAxis.normalized;
            if (angle > 360.0f) angle = -360.0f;
            lastAngle = angle;
        }
        protected void endPointCheck()
        {
            if (Controller.Position != movementData.EndPoint) return;
            isLock = true;
            if (Step.IsMovementStep() && !Step.IsDirectMovement())
            {
                HeroStep_Change(Step.ToDirectMovement());
            }
            else HeroStep_Change(HeroSteps.Idle);
        }
        protected bool obstacleDetection_AnotherHero(HeroController anotherHeroController)
        {
            switch (anotherHeroController.inputController.controllerType)
            {
                case InputManager.ControllerType.CPU:
                    if (anotherHeroController.lineSensorController.CanMovingInDirection(movementAxis.MotionAxis))
                    {
                        anotherHeroController.inputController.AddForce(movementAxis.MotionAxis);
                        return true;
                    }
                    break;
                case InputManager.ControllerType.Human:
                    if (anotherHeroController.lineSensorController.CanMovingInDirection(movementAxis.MotionAxis))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        protected bool obstacleDetection_Terrain() => obstacleDetection_Terrain(Step);
        protected bool obstacleDetection_Terrain(HeroSteps step)
        {
            RaycastHit hit;
            if (!Controller.lineSensorController.Radiation(Vector3.up, out hit))// check up -> no obstacle
            {
                if (Controller.lineSensorController.UpRadiation(movementAxis.MotionAxis, out hit))
                {
                    if (hit.collider.GetComponent<CollectableObject>()|| hit.collider.GetComponent<InformerHandler>())
                    {
                        HeroStep_ChangeGoUpNext(step);
                        return true;
                    }
                    return false;
                }
                else // check follow motion -> no obstacle
                {
                    HeroStep_ChangeGoUpNext(step);
                    return true;
                }
            }
            return false;
        }
        private void HeroStep_ChangeGoUpNext(HeroSteps step)
        {
            isLock = true;
            if (step == HeroSteps.GoForward) HeroStep_Change(HeroSteps.GoUpForward);
            else if (step == HeroSteps.GoBack) HeroStep_Change(HeroSteps.GoUpBack);
            else if (step == HeroSteps.GoRight) HeroStep_Change(HeroSteps.GoUpRight);
            else if (step == HeroSteps.GoLeft) HeroStep_Change(HeroSteps.GoUpLeft);
        }
        protected void calculation()
        {
            Vector3 p1 = Controller.Position;
            Vector3 p2 = Controller.Position + movementAxis.MotionAxis * getStepSize();
            movementData = new MovementData(p1, p2);
            distance = Vector3.Distance(movementData.EndPoint, movementData.StartPoint);
            _isMovementSuccessfully = true;
        }
        protected override void Reset()
        {
            _isMovementSuccessfully = false;
            lastAngle = 0;
            isLock = false;
        }
        protected void checkMoveTowardsToObstacle()
        {
            Door door;
            if (!Controller.lineSensorController.Radiation(movementAxis.MotionAxis, out var hit))
            {
                if (Controller.lineSensorController.RadiationToGround(movementAxis.MotionAxis, out hit, Mathf.Infinity))
                {
                    if ((door = hit.collider.GetComponent<Door>()) != null)
                    {
                        Controller.inventoryController.MakeTrade(door);
                        HeroStep_Change(HeroSteps.Idle);
                        return;
                    }
                }
                else if (!settings.EnableFalling)
                {
                    HeroStep_Change(HeroSteps.Idle);
                    return;
                }
                calculation();
                return;
            }
            //if (!Step.IsDirectMovement() || Step != HeroSteps.Sliding) return;
            door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                Controller.inventoryController.MakeTrade(door);
                HeroStep_Change(HeroSteps.Idle);
                return;
            }
            //
            Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                var portal = hit.collider.GetComponent<Portal>();
                if (portal != null)
                {
                    if (portal.CanHostReception(Controller.gameObject))
                    {
                        calculation();
                        return;
                    }
                }
                if (obstacle.IsClimbable && obstacleDetection_Terrain()) { return; }
                else
                {
                    HeroStep_Change(HeroSteps.Idle);
                    return;
                }
            }
            //
            HeroController anotherHero = hit.collider.GetComponent<HeroController>();
            if (anotherHero != null)
            {
                if (obstacleDetection_AnotherHero(anotherHero))
                {
                    calculation();
                    return;
                }
                if (!Controller.lineSensorController.UpRadiation(movementAxis.MotionAxis, out hit))
                {
                    HeroStep_Change(Step.ToUpAndMoving());
                    return;
                }
                HeroStep_Change(HeroSteps.Idle);
                return;
            }
            //
            CollectableObject collectable = hit.collider.GetComponent<CollectableObject>();
            if (collectable != null)
            {
                calculation();
                return;
            }
            calculation();
            //HeroStep_Change(HeroSteps.Idle);
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct MovementAxis
        {
            public Vector3 MotionAxis;
            public Vector3 RotationAxis;
            public MovementAxis(Vector3 motionAxis, Vector3 rotationAxis)
            {
                this.MotionAxis = motionAxis;
                this.RotationAxis = rotationAxis;
            }
        }
        [System.Serializable]
        public struct MovementData
        {
            public Vector3 StartPoint;
            public Vector3 EndPoint;
            public MovementData(Vector3 startPoint, Vector3 endPoint)
            {
                this.StartPoint = startPoint;
                this.EndPoint = endPoint;
            }
        }
        #endregion
    }
}