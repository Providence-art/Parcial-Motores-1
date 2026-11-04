#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    using UnityEditor;
    using UnityEngine;

    public class TextWindow : EditorWindow
    {
        public static void OpenWindow(string title, string descriptions) => OpenWindow(title, new string[] { descriptions });
        public static void OpenWindow(string title, string[] descriptions)
        {
            TextWindow window = (TextWindow)EditorWindow.GetWindow(typeof(TextWindow), true, title);
            window.Descriptions = descriptions;
        }
        public string Title = string.Empty;
        public string[] Descriptions;
        void OnGUI()
        {
            GUILayout.Label(Title, EditorStyles.boldLabel);
            if (Descriptions != null)
            {
                for (int i = 0; i < Descriptions.Length; i++)
                {
                    GUILayout.Label(string.IsNullOrEmpty(Descriptions[i]) ? string.Empty : Descriptions[i]);
                }
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
        }
    }
}
#endif