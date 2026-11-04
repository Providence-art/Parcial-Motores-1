#if UNITY_EDITOR
using HashGame.CubeWorld.Extensions;
using UnityEngine;

namespace HashGame.CubeWorld
{
    public static class PrefabManager
    {
        private const string HeroPrefabRootFilePath = Globals.PublisherName + "/" + Globals.ProjectName + "/Resources/Prefabs/Hero";
        public const string HeroPrefabFilePath_Default = HeroPrefabRootFilePath + "/Default";
        public const string HeroPrefabFilePath_Rubiks = HeroPrefabRootFilePath + "/Rubik";
        public const string HeroPrefabFilePath_Characters = HeroPrefabRootFilePath + "/Characters";
        public static GameObject[] LoadHeroPrefabs(string path = HeroPrefabFilePath_Default) => PathExtentions.FetchPrefabs(path);
    }
}
#endif