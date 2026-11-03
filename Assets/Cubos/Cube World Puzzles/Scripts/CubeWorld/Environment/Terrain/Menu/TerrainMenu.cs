#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class TerrainMenu : MainMenu
    {
        public const string NAME = MainMenu.MenuItemProjectName + "/Terrain/";
        [MenuItem(NAME + "/Select or Add terrain")]
        public static void SelectOrAddTrain()
        {
            GameObject obj;
            var item = GameObject.FindFirstObjectByType<TerrainManager>();
            if (item == null)
            {
                obj = new GameObject("Terrain");
                obj.AddComponent<TerrainManager>();
                //obj.AddComponent<TerrainEditorData>();
                Undo.RegisterCreatedObjectUndo(obj, "Create terrain manager");
                obj.transform.position = getPosition();
            }
            else
            {
                obj = item.gameObject;
            }
            Selection.objects = new Object[] { obj };
        }
        private static Vector3 getPosition()
        {
            var item = GameObject.FindFirstObjectByType<TerrainManager>();
            if (item == null) return Vector3.zero;
            return item.transform.position;
        }
    }
}
#endif