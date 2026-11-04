namespace HashGame.CubeWorld
{
    using UnityEngine;

    public class TerrainData : MonoBehaviour
    {
        #region variables
        public GameObject Max_X { get; protected set; }
        public GameObject Min_X { get; protected set; }
        public GameObject Max_Y { get; protected set; }
        public GameObject Min_Y { get; protected set; }
        public GameObject Max_Z { get; protected set; }
        public GameObject Min_Z { get; protected set; }
        #endregion
        #region functions
        public void CheckCube(GameObject obj)
        {
            if (obj == null) return;
            checkMinMax_X(obj);
            checkMinMax_Y(obj);
            checkMinMax_Z(obj);
        }
        private void checkMinMax_X(GameObject obj)
        {
            if (!obj) return;
            if (obj.transform.parent != this.transform) return;
            float x = obj.transform.position.x;
            if (Min_X == null || Min_X.transform.position.x > x)
            {
                Min_X = obj;
            }
            if (Max_X == null || Max_X.transform.position.x < x)
            {
                Max_X = obj;
            }
        }
        private void checkMinMax_Y(GameObject obj)
        {
            if (!obj) return;
            if (obj.transform.parent != this.transform) return;
            float y = obj.transform.position.y;
            if (Min_Y == null || Min_X.transform.position.y > y)
            {
                Min_Y = obj;
            }
            if (Max_Y == null || Max_X.transform.position.y < y)
            {
                Max_Y = obj;
            }
        }
        private void checkMinMax_Z(GameObject obj)
        {
            if (!obj) return;
            if (obj.transform.parent != this.transform) return;
            float z = obj.transform.position.z;
            if (Min_Z == null || Min_X.transform.position.z > z)
            {
                Min_Z = obj;
            }
            if (Max_Z == null || Max_X.transform.position.z < z)
            {
                Max_Z = obj;
            }
        }
        #endregion
    }
}