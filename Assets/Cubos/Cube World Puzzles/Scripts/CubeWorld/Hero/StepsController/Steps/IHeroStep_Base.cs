using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;

namespace HashGame.CubeWorld.HeroManager
{
    [System.Serializable]
    public abstract class IHeroStep_Base : IHeroStep
    {
        public IHeroStep_Base(HeroController controller, HeroSteps step)
        {
            this.Step = step;
            this.Controller = controller;
        }
        #region variable
        public HeroSteps Step { get; }
        public HeroController Controller { get; }
        public HeroController.HeroControllerEvents events => Controller.events;
        public HeroController.HeroControllerBuffer buffer => Controller.buffer;

        protected bool isLock = false;
        #endregion
        #region abstract
        public abstract void OnStepStart();
        public abstract void OnStepFinish();
        public abstract void OnUpdate();
        #endregion
        #region functions
        protected void HeroStep_Change(HeroSteps step)
        {
            isLock = true;
            Controller.ChangeStep(step);
        }
        protected virtual void Reset()
        {
            isLock = false;
        }
        protected bool CheckIsGrounded()
        {
            if (!Controller.StayOnGround())
            {
                HeroStep_Change(HeroSteps.Falling);
                return false;
            }
            return true;
        }
        protected void CheckGroundType()
        {
            RaycastHit[] hits = Controller.lineSensorController.Radiation_RaycastAll(Vector3.down);
            if (hits.Length == 0) return;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) continue;
                Obstacle ground = hits[i].collider.GetComponent<Obstacle>();
                if (ground == null) continue;
                ground.ObjectIsStanding(Controller.gameObject);
                if (ground.obstacleType == ObstacleType.Slide)
                {
                    StayOnSlideObstacle();
                }
            }
        }
        protected virtual void StayOnSlideObstacle()
        {
            HeroSteps lastStep = Controller.movementSteps.Last;
            if (!IHeroStep_Movement.MovementAxesTable.ContainsKey(lastStep)) return;
            RaycastHit hit;
            if (Controller.lineSensorController.Radiation(((IHeroStep_Movement.MovementAxis)IHeroStep_Movement.MovementAxesTable[lastStep]).MotionAxis, out hit))
            {
                if (hit.collider != null &&
                    (hit.collider.gameObject.GetComponent<Obstacle>()
                    || hit.collider.gameObject.GetComponent<Door>())) return;
            }
            HeroStep_Change(HeroSteps.Sliding);
        }
        protected void AudioClipPlaySFX() => AudioClipPlaySFX(Step);
        protected void AudioClipPlaySFX(HeroSteps step)
        {
            if (!Controller.isActiveAndEnabled) return;
            if (step.IsMovementStep())
            {
                AudioManager.Instance.PlaySFX(Controller.AudioClips.FootStep);
                return;
            }
            switch (step)
            {
                case HeroSteps.Sliding:
                    break;
                case HeroSteps.Falling:
                    AudioManager.Instance.PlaySFX(Controller.AudioClips.Falling);
                    break;
                case HeroSteps.Destroy:
                    AudioManager.Instance.PlaySFX(Controller.AudioClips.Destroying);
                    break;
            }

        }
        #endregion
    }
}