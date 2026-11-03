namespace HashGame.CubeWorld.TerrainConstruction
{
    public class TerrainLand_Quadrilateral : TerrainLand
    {
        public int width;
        public int length;
        public int maxHeight;
        public float heightRate = .1f;
        protected float constructionFactor = .8f;
        public void setHeightRate(float value)
        {
            getCoefficient(ref heightRate, value);
        }
        public void setConstructionFactor(float value)
        {
            getCoefficient(ref constructionFactor, value);
        }
        private void getCoefficient(ref float coefficient, float value)
        {
            if (value >= MIN_C && value <= MAX_C)
            {
                coefficient = value;
            }
        }
        public bool Create(ref bool[] map, bool trueMeansObstacleExist = true)
        {
            if (width <= 0 && length <= 0) return false;
            if (map == null || map.Length != width * length) return false;
            InitLand(width, length);
            for (int i = 0; i < base.i; i++)
            {
                for (int j = 0; j < base.j; j++)
                {
                    if (trueMeansObstacleExist == map[Index(i, j)])
                    {
                        int count = 1;
                        if (UnityEngine.Random.Range(MIN_C, MAX_C) < heightRate)
                        {
                            count = (int)UnityEngine.Random.Range(count, maxHeight + 1);
                        }
                        base.AddData(i, j, new SurfaceData(0, SurfaceDataDirection.Up, count));
                    }
                }
            }
            return true;
        }
        public bool Create()
        {
            if (width <= 0 && length <= 0) return false;
            InitLand(width, length);
            for (int i = 0; i < base.i; i++)
            {
                for (int j = 0; j < base.j; j++)
                {
                    if (UnityEngine.Random.Range(MIN_C, MAX_C) < constructionFactor)
                    {
                        int count = 1;
                        if (UnityEngine.Random.Range(MIN_C, MAX_C) < heightRate)
                        {
                            count = (int)UnityEngine.Random.Range(count, maxHeight + 1);
                        }
                        base.AddData(i, j, new SurfaceData(0, SurfaceDataDirection.Up, count));
                    }
                }
            }
            return true;
        }
    }
}