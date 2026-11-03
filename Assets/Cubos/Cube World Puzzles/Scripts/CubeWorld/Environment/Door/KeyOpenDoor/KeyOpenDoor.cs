using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(Door))]
    public class KeyOpenDoor : DoorTypeBase
    {
        #region variable
        [HideInInspector] public Door door;
        public LocalSettings localSettings = new LocalSettings()
        {
            OpenKeyCount = 1,
        };
        #endregion
        #region Functions
        private void OnValidate()
        {
            door = GetComponent<Door>();
        }
        #endregion
        #region Key
        public bool NeedKey() => localSettings.NeedKey();
        public void KeyIsOnWay() => localSettings.openKeyOnWayCount++;
        public void KeyRecive(CollectableObject obj)
        {
            localSettings.CurrentKeyCount++;
            localSettings.openKeyOnWayCount--;
            if (localSettings.CanOpen())
            {
                door.ChangeState(DoorState.Opening);
            }
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct LocalSettings
        {
            [Min(1)]
            public int OpenKeyCount;
            public int CurrentKeyCount;// { get; set; }
            public int openKeyOnWayCount { get; set; }
            public bool CanOpen() => OpenKeyCount <= (CurrentKeyCount + openKeyOnWayCount);
            public bool NeedKey() => !CanOpen();
        }
        #endregion
    }
}