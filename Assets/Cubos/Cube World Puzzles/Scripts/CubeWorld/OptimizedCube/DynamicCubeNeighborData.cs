using System;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DynamicCubeNeighborData : MonoBehaviour
    {
        public bool Radiation(CubeFace face, out RaycastHit hit)
        {
            Ray ray = new Ray(transform.position, DynamicCubeFaceRendering.getDirection(face));
            return Physics.Raycast(ray, out hit, DynamicCubeFaceRendering.SIZE);
        }
        public DynamicCubeFaceRendering getNeighbor(CubeFace face)
        {
            Ray ray = new Ray(transform.position, DynamicCubeFaceRendering.getDirection(face));
            DynamicCubeFaceRendering result = default(DynamicCubeFaceRendering);
            if (Physics.Raycast(ray, out var hit, DynamicCubeFaceRendering.SIZE))
            {
                if (hit.collider != null && (result = hit.collider.gameObject.GetComponent<DynamicCubeFaceRendering>()))
                {
                    return result;
                }
            }
            return result;
        }
        public bool HasNeighbor(CubeFace face)
        {
            Ray ray = new Ray(transform.position, DynamicCubeFaceRendering.getDirection(face));

            if (Physics.Raycast(ray, out var hit, DynamicCubeFaceRendering.SIZE))
            {
                if (hit.collider != null && hit.collider.gameObject.GetComponent<DynamicCubeFaceRendering>())
                {
                    return true;
                }
            }
            return false;
        }
        public void SetNeighbor(CubeFace face, GameObject neighbor)
        {
            //neighbors[(int)face] = neighbor;
        }
        public T GetNeighbor<T>(CubeFace face) where T : DynamicCubeFaceRendering
        {
            return GetNeighbor<T>(face.ToVector3());
        }
        public T GetNeighbor<T>(Vector3 direction) where T : DynamicCubeFaceRendering
        {
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out var hit, Obstacle.SIZE))
            {
                if (hit.collider != null)
                {
                    return hit.collider.gameObject.GetComponent<T>();
                }
            }
            return null;
        }

        public void CheckNeighbors()
        {
            Obstacle obstacle = GetComponent<Obstacle>();
            if (obstacle != null && !obstacle.IsOptimizable) return;
            if (!obstacle.obstacleType.CanDisableFace())
            {
                obstacle.DisplayeAllFaces(true);
                return;
            }
            DynamicCubeFaceRendering cube = GetComponent<DynamicCubeFaceRendering>();
            if ((cube) == null) return;
            foreach (var node in Enum.GetValues(typeof(CubeFace)))
            {
                CheckNeighbor((CubeFace)node, false);
            }
            cube.UpdateIncludeFaces();
        }
        public bool CheckNeighbor(CubeFace face, bool forceUpdate = true)
        {
            DynamicCubeFaceRendering cube = GetComponent<DynamicCubeFaceRendering>();
            if ((cube) == null) return false;
            bool result = HasNeighbor(face);
            cube.DisplayFace(face, !result, forceUpdate);
            return result;
        }
        public Vector3 getLogicalNeighborPosition(CubeFace face)
        {
            DynamicCubeFaceRendering cube = GetComponent<DynamicCubeFaceRendering>();
            if (cube == null) return Vector3.zero;
            return cube.Position + DynamicCubeFaceRendering.getDirection(face) * 2 * cube.getSize(face);
        }
        public void FixPositionFromSide(CubeFace face)
        {
            Ray ray = new Ray(transform.position, Obstacle.getDirection(face));
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider == null) return;
            Obstacle neighbor = hit.collider.gameObject.GetComponent<Obstacle>();
            if (neighbor == null) return;
            Obstacle me = gameObject.GetComponent<Obstacle>();
            if (me == null) return;
            CubeFace oppositeFace = face.ToOpposite();
            Vector3 newPosition = neighbor.Position
                + Obstacle.getDirection(oppositeFace)
                * (neighbor.getSize(oppositeFace) + me.getSize(face));
            transform.position = newPosition;
        }
        public void ChangeMyNeighborsSideMeFaceDisplay(bool enable)
        {
            foreach (CubeFace face in Enum.GetValues(typeof(CubeFace)))
            {
                Obstacle neighbor = GetNeighbor<Obstacle>(face);
                if (neighbor == null) continue;
                neighbor.DisplayFace(face.ToOpposite(), enable);
            }
        }
    }
}