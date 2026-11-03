using System.IO;
using UnityEngine;
namespace HashGame.CubeWorld.RenderingZone
{
    public class HighResScreenshot : MonoBehaviour
    {
        public int resolutionMultiplier = 2;
        public KeyCode ScreenShotKey= KeyCode.Space;

        void Update()
        {
            if (Input.GetKeyDown(ScreenShotKey))
            {
                TakeScreenshot();
            }
        }

        void TakeScreenshot()
        {
            string folderPath = Path.Combine(Application.dataPath, "Screenshots");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string screenshotPath = Path.Combine(folderPath, "screenshot_" + timestamp + ".png");

            int width = Screen.width * resolutionMultiplier;
            int height = Screen.height * resolutionMultiplier;
            ScreenCapture.CaptureScreenshot(screenshotPath, resolutionMultiplier);
            Debug.Log("Screenshot saved to: " + screenshotPath);
        }
    }
}