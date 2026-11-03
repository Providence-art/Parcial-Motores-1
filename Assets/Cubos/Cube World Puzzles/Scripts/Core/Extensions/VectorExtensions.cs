namespace HashGame.CubeWorld.Extensions
{
    using UnityEngine;
    public static class VectorExtensions
    {
        public static Vector3 ToCenter(Vector3 v1, Vector3 v2) => ToCenter(new Vector3[] { v1, v2 });
        public static Vector3 ToCenter(this Vector3[] vectors)
        {
            Vector3 sum = Vector3.zero;
            if (vectors == null || vectors.Length < 1) return sum;
            for (int i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }
            return sum / vectors.Length;
        }
        public static Vector3 MakeClone(this Vector3 v) => new Vector3(v.x, v.y, v.z);
        #region X,Y
        public static Vector2 ToXY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector2[] ToXY_array(this Vector3[] v)
        {
            Vector2[] result = new Vector2[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                result[i] = (v[i].ToXY());
            }
            return result;
        }
        #endregion
        #region X,Z
        public static Vector2 ToXZ(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2[] ToXZ_array(this Vector3[] v)
        {
            Vector2[] result = new Vector2[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                result[i] = (v[i].ToXZ());
            }
            return result;
        }
        #endregion
        #region X,Y
        public static Vector2 ToYZ(this Vector3 v) => new Vector2(v.y, v.z);
        public static Vector2[] ToYZ_array(this Vector3[] v)
        {
            Vector2[] result = new Vector2[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                result[i] = (v[i].ToYZ());
            }
            return result;
        }
        #endregion

    }
}