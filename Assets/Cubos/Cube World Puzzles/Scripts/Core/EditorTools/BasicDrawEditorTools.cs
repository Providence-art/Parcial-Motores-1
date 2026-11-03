#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace HashGame.CubeWorld.EditorTools
{
    public static class BasicDrawEditorTools
    {
        #region Lable
        public static void Lable(Vector3 position, string text)
        {
            Handles.Label(position, text);
        }
        public static void Lable(Vector3 position, string text, UnityEngine.Color color)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = color;
            Handles.Label(position, text, style);
        }
        #endregion
        public static void Repaint() => SceneView.RepaintAll();

    }
}
#endif