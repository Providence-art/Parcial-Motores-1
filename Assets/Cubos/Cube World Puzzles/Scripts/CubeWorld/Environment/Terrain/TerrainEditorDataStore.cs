#if UNITY_EDITOR
namespace HashGame.CubeWorld
{
    using HashGame.CubeWorld.TerrainConstruction;
    using UnityEngine;
    public class TerrainEditorDataStore : MonoBehaviour
    {
        private void OnValidate()
        {
            if (!settings) settings = TerrainEditorSettings.Instance;
        }
        public TerrainEditorSettings settings;
    }
}
#endif