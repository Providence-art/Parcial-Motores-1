using System.Collections.Generic;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Undercover_Stealth : IUndercover
    {
        public Undercover_Stealth(Undercover controller)
        {
            Controller = controller;
        }

        public UndercoverStates Steps => UndercoverStates.Stealth;

        public Undercover Controller { get; }

        public void ObjectIsStanding(GameObject obj)
        {
            if (obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (hero == null) return;
            Controller.ChangeState(UndercoverStates.Identify);
        }

        public void OnStepFinish()
        {
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null)
                Controller.obstacle.terrainManager.PlaySFX(Controller.MyObstacleType);
        }

        public void OnStepStart()
        {
            Controller.obstacle.DisplayeAllFaces(false);
            List<CubeFace> faces = new List<CubeFace>();
            foreach (CubeFace face in System.Enum.GetValues(typeof(CubeFace)))
            {
                checkNeighborFaces(face);
            }
        }
        private void checkNeighborFaces(CubeFace face)
        {
            DynamicCubeFaceRendering obj = Controller.obstacle.neighborData.getNeighbor(face);
            if (obj == null) return;
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            if (obstacle && obstacle.obstacleType == ObstacleType.Undercover) return;
            obj.DisplayFace(face.ToOpposite(), true);
        }
    }
}