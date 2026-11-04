using UnityEngine;

namespace HashGame.CubeWorld.TerrainConstruction
{
    [System.Serializable]
    public class DungeonGenerator
    {
        private int _width = 50;
        public int Width
        {
            get => _width;
            set => _width = System.Math.Max(1, value);
        }
        private int _height = 50;
        public int Height
        {
            get => _height;
            set => _height = System.Math.Max(1, value);

        }
        public int minRoomSize = 4;
        public int maxRoomSize = 10;
        public int maxRooms = 10;
        //public GameObject wallTile; 
        //public GameObject floorTile; 
        public bool isGenerated { get; private set; }
        private bool[,] map;
        public bool[,] Map => map;

       public  void GenerateMap()
        {
            map = new bool[Width, Height];

            for (int i = 0; i < maxRooms; i++)
            {
                int roomWidth = Random.Range(minRoomSize, maxRoomSize + 1);
                int roomHeight = Random.Range(minRoomSize, maxRoomSize + 1);
                int roomX = Random.Range(0, Width - roomWidth);
                int roomY = Random.Range(0, Height - roomHeight);

                if (!IsRoomOverlapping(roomX, roomY, roomWidth, roomHeight))
                {
                    CreateRoom(roomX, roomY, roomWidth, roomHeight);

                    if (i > 0)
                    {
                        ConnectRooms(i);
                    }
                }
            }
            isGenerated = true;
        }

        bool IsRoomOverlapping(int x, int y, int width, int height)
        {
            for (int i = x - 1; i < x + width + 1; i++)
            {
                for (int j = y - 1; j < y + height + 1; j++)
                {
                    if (i >= 0 && i < Width && j >= 0 && j < Height && map[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void CreateRoom(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    map[i, j] = true;
                }
            }
        }

        void ConnectRooms(int currentRoomIndex)
        {
            int prevRoomCenterX = Random.Range(0, Width);
            int prevRoomCenterY = Random.Range(0, Height);
            int currentRoomCenterX = Random.Range(0, Width);
            int currentRoomCenterY = Random.Range(0, Height);

            CreateCorridor(prevRoomCenterX, prevRoomCenterY, currentRoomCenterX, currentRoomCenterY);
        }

        void CreateCorridor(int startX, int startY, int endX, int endY)
        {
            while (startX != endX)
            {
                map[startX, startY] = true;
                startX += (startX < endX) ? 1 : -1;
            }

            while (startY != endY)
            {
                map[startX, startY] = true;
                startY += (startY < endY) ? 1 : -1;
            }
        }
        void DrawDungeon()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    //Vector3 position = new Vector3(x, 0, y);
                    //GameObject tile = Map[x, y] ? floorTile : wallTile;
                    //Instantiate(tile, position, Quaternion.identity);
                }
            }
        }

    }
}