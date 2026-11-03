#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HashGame.CubeWorld.Extensions
{
    public static class PathExtentions
    {
        public static GameObject[] FetchPrefabs(string folderPath = "Resources")// folderPath = "Resources/Prefabs
        {
            List<GameObject> prefabs = new List<GameObject>();
            string dir = Path.Combine(Application.dataPath, folderPath);
            string[] prefabPaths = Directory.GetFiles(dir, "*.prefab");
            foreach (string path in prefabPaths)
            {
                string relativePath = "Assets" + path.Replace(Application.dataPath, "").Replace('\\', '/');

                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);

                if (prefab == null) continue;
                //Instantiate(prefab, transform);
                prefabs.Add(prefab);
            }
            return prefabs.ToArray();
        }
    }
}
#endif