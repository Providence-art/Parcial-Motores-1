#if UNITY_EDITOR
using HashGame.CubeWorld.CameraSystem;
using HashGame.CubeWorld.Extensions;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class MenuCamera : MainMenu
    {
        public const string NAME = MainMenu.MenuItemProjectName + "/Camera/";
        //

        [MenuItem(NAME + "/Select (current camera)")]
        public static void SelectCurrentCamera()
        {
            if (!BasicExtensions.Camera)
            {
                NoCameraFoundWindow();
                return;
            }
            CameraBasic item = GameObject.FindFirstObjectByType<CameraBasic>();
            if (item == null)
            {
                NoCameraFoundWindow();
            }
            else
            {
                Selection.objects = new Object[] { item.gameObject };
            }
        }
        //
        [MenuItem(NAME + IsometricCameraController.NAME + " /Select or Add")]
        public static void SelectOrAddIsometricCamera() => selectOrAddIsometricCamera<IsometricCameraController>();
        //
        [MenuItem(NAME + SmoothCameraFollow.NAME + " /Select or Add")]
        public static void SelectOrAddSmoothCameraFollow() => selectOrAddIsometricCamera<SmoothCameraFollow>();
        //
        private static void selectOrAddIsometricCamera<T>() where T : CameraBasic
        {
            if (!BasicExtensions.Camera)
            {
                NoCameraFoundWindow();
                return;
            }
            CameraBasic item = GameObject.FindFirstObjectByType<CameraBasic>();
            if (item == null)
            {
                Undo.AddComponent(BasicExtensions.Camera.gameObject, typeof(T));
                Selection.objects = new Object[] { BasicExtensions.Camera.gameObject };
            }
            else if (item.GetType() == typeof(T))
            {
                Selection.objects = new Object[] { item.gameObject };
            }
            else
            {
                Undo.DestroyObjectImmediate(item);
                Undo.AddComponent(BasicExtensions.Camera.gameObject, typeof(T));
                Selection.objects = new Object[] { BasicExtensions.Camera.gameObject };
            }
        }
        public static void NoCameraFoundWindow() => TextWindow.OpenWindow("Error", "No camera found.");
    }
}
#endif