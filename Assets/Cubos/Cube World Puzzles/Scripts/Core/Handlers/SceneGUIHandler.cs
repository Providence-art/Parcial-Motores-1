#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    using System.Net.NetworkInformation;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class SceneGUIHandler
    {
        public static bool CameraDirection(out Vector3 vector)
        {
            Camera sceneCamera = SceneView.lastActiveSceneView.camera;
            if (sceneCamera != null)
            {
                Vector3 cameraForward = sceneCamera.transform.forward;
                vector = cameraForward;
                Vector3 cameraEulerAngles = sceneCamera.transform.eulerAngles;
                return true;
            }
            vector = default(Vector3);
            return false;
        }
        #region mouse handler
        public static Vector3 getMousePosition() => getMousePosition(Event.current);
        public static Vector3 getMousePosition(Event e)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            float drawPlaneHeight = 0;
            float distanceToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
            var result = mouseRay.GetPoint(distanceToDrawPlane);
            return result;
        }
        public bool MouseRightClick() => MouseRightClick(Event.current);
        public bool MouseRightClick(Event e) => e.type == EventType.MouseDown && e.button == 1;
        //
        public bool MouseLeftClick() => MouseLeftClick(Event.current);
        public bool MouseLeftClick(Event e) => e.type == EventType.MouseDown && e.button == 0;
        //
        public bool MouseMove() => MouseMove(Event.current);
        public bool MouseMove(Event e) => e.type == EventType.MouseMove;
        //
        public static GameObject MouseIsHoveringOver() => MouseIsHoveringOver(Event.current);
        public static GameObject MouseIsHoveringOver(Event e)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(controlID);
            }
            if (e.type == EventType.MouseMove)
            {
                GameObject hoveredObject = HandleUtility.PickGameObject(e.mousePosition, false);
                return hoveredObject;
            }
            return null;
        }
        //
        public static GameObject MouseHitAnObject() => MouseHitAnObject(Event.current);
        public static GameObject MouseHitAnObject(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        //
        public static GameObject MouseIsOverAnObject() => MouseIsOverAnObject(Event.current);
        public static GameObject MouseIsOverAnObject(Event e)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(controlID);
            }
            return HandleUtility.PickGameObject(e.mousePosition, false);
        }
        //
        public static void MouseOverPingObject() => MouseOverPingObject(Event.current);
        public static void MouseOverPingObject(Event e)
        {
            GameObject hoveredObject = HandleUtility.PickGameObject(e.mousePosition, false);
            if (hoveredObject != null)
            {
                EditorGUIUtility.PingObject(hoveredObject);
            }
        }
        //
        public static bool MouseOverGUILayoutUtility(ref Rect rect) => MouseOverGUILayoutUtility(ref rect, Event.current);
        public static bool MouseOverGUILayoutUtility(ref Rect rect, Event e) => rect.Contains(e.mousePosition);
        //
        public static bool isMoseMove() => isMoseMove(Event.current);
        public static bool isMoseMove(Event e) => (e.type == EventType.MouseMove);
        public static bool isMouseRightClick(Event e) => (e.type == EventType.MouseDown && e.button == 1);
        public static bool isMouseLeftClick(Event e) => (e.type == EventType.MouseDown && e.button == 0);
        public static bool isMouseLeftClickAndDrag(Event e) => (e.type == EventType.MouseDrag && e.button == 0);
        #endregion
    }
}
#endif