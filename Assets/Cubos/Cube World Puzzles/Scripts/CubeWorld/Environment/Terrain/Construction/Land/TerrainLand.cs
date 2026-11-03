namespace HashGame.CubeWorld.TerrainConstruction
{
    [System.Serializable]
    public class TerrainLand
    {
        public const float MAX_C = 1.0f;
        public const float MIN_C = 0.0f;
        protected int i;
        protected int j;
        public SurfaceData[][] land;
        public void InitLand(int i, int j)
        {
            this.i = i;
            this.j = j;
            land = new SurfaceData[this.i * this.j][];
        }
        public int Index(int i, int j)
        {
            if (i >= 0 && i < this.i && j >= 0 && j < this.j)
            {
                return i * this.j + j;
            }
            return -1;
        }
        public void AddData(int i, int j, SurfaceData data) => AddData(i, j, new SurfaceData[] { data });
        public void AddData(int i, int j, SurfaceData[] data)
        {
            if (i >= 0 && i < this.i && j >= 0 && j < this.j)
            {
                land[i * this.j + j] = data;
            }
        }
        public SurfaceData[] GetData(int i, int j)
        {
            if (i >= 0 && i < this.i && j >= 0 && j < this.j)
            {
                return land[i * this.j + j];
            }
            return null;
        }
        #region struct
        public enum SurfaceDataDirection : int
        {
            Down = -1,
            Up = 1
        }
        public struct SurfaceData
        {
            public int Level;
            public SurfaceDataDirection Direction;
            public int Count;
            public SurfaceData(int level, SurfaceDataDirection direction, int count)
            {
                Level = level;
                Direction = direction;
                Count = count;
            }
        }
        #endregion
    }
}