#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class MainMenu : MonoBehaviour
    {
        public const string MenuItemPublisherName = "Tools/" + Globals.PublisherName;
        public const string MenuItemProjectName = MenuItemPublisherName + "/" + Globals.ProjectName;
        #region About us
        [MenuItem(MenuItemPublisherName + "/Publisher Page")]
        public static void PublisherPage()
        {
            Application.OpenURL(Globals.PublisherPage);
        }
        #endregion
        #region media
        [MenuItem(MenuItemPublisherName + "/YouTube")]
        public static void YouTubePage()
        {
            Application.OpenURL(Globals.YouTube);
        }
        #endregion
        #region Support
        [MenuItem(MenuItemPublisherName + "/Support")]
        public static void Support()
        {
            TextWindow window = (TextWindow)EditorWindow.GetWindow(typeof(TextWindow), true, "Support");
            window.Descriptions = new string[] { "If you need any further assistance, please contact us",
                "Email: "+Globals.SupportEmail,
                "YouTube: "+Globals.YouTube,
                "Thank you." };
        }
        #endregion
    }
}
#endif