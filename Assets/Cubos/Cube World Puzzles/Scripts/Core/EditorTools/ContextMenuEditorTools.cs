#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public static class ContextMenuEditorTools
    {
        // use in OnSceneGUI();
        public static GenericMenu genericMenu() => new GenericMenu();
        public static void AddItem(ref GenericMenu menu, string name, Action function = null)
        {
            menu.AddItem(new GUIContent(name), false, () => { if (function != null) function.Invoke(); });
        }
        public static void SeparatorItem(ref GenericMenu menu, string path = "") => menu.AddSeparator(path);
        public static void OpenMenuInMousePosition(ref GenericMenu menu) => menu.ShowAsContext();
        public static void UseEventAtTheEnd() => UseEventAtTheEnd(Event.current);
        public static void UseEventAtTheEnd(Event e) => e.Use();
        // don`t forgot to e.Use();
        public static void CreateHelloWorld()
        {
            GenericMenu menu = genericMenu();
            AddItem(ref menu, "Hello world.", () => { });
            OpenMenuInMousePosition(ref menu);
            UseEventAtTheEnd();
        }
    }
}
#endif