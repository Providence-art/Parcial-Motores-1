using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HashGame.CubeWorld.Extensions
{
    public static class BasicExtensions
    {   
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         UnityEngine.Application.Quit();
#endif
        }
        public static void RemoveClass<T>(this GameObject obj) where T : MonoBehaviour
        {
            if (!obj) return;
            T node = obj.GetComponent<T>();
            if (node == null) return;
            DestroyObject(node);
        }
        public static void DestroyObject(Object obj)
        {
            if (obj == null) return;
            if (Application.isPlaying)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                GameObject.DestroyImmediate(obj);
            }
        }
        private static Camera _camera;
        public static Camera Camera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }
        private static readonly Dictionary<float, WaitForSeconds> _waitDictionary = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWaiteForSeconds(float time)
        {
            if (_waitDictionary.TryGetValue(time, out var wait)) return wait;
            return (_waitDictionary[time] = new WaitForSeconds(time));
        }
        private static PointerEventData _pointerEventDataCurrentPosition;
        private static List<RaycastResult> _raycastResult;
        public static bool IsOverUI()
        {
            _pointerEventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _raycastResult = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_pointerEventDataCurrentPosition, _raycastResult);
            bool result = _raycastResult.Count > 0;
            _raycastResult.Clear();
            return result;
        }
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform node)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(node, node.position, Camera, out var result);
            return result;
        }
        public static void DeleteChildren(this Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                DestroyObject(t.GetChild(i).gameObject);
            }
            //foreach (Transform child in t) DestroyObject(child.gameObject);
        }
    }
}