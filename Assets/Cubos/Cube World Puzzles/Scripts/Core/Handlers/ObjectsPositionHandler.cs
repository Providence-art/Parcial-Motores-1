using System.Collections.Generic;
using UnityEngine;
namespace HashGame.CubeWorld
{
    [System.Serializable]
    public class ObjectsPositionHandler
    {
        public List<GameObject> cubes;
        [SerializeField] private Dictionary<Vector3, GameObject> cubePositions;
        public bool AddObject(GameObject obj, bool forceOverride = false)
        {
            if (obj == null) return false;
            if (IsExist(obj) && !forceOverride) return false;
            cubePositions[obj.transform.position] = obj;
            return true;
        }
        public void RemoveObject(GameObject obj)
        {
            if (obj == null) return;
            cubePositions.Remove(obj.transform.position);
        }
        public bool IsExist(GameObject obj) => obj != null && cubePositions.ContainsKey(obj.transform.position);
    }
}