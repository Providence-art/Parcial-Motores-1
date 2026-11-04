using UnityEngine;
using UnityEngine.UI;
namespace HashGame
{

    public class GameManager : MonoBehaviour
    {
        public Text resultText;
        public GameObject menuPanel;

        private void Awake()
        {
            menuPanel.SetActive(false);
        }
        public void ShowWinMenu() => ShowResult(true);
        public void ShowResult(bool isWinner)
        {
            menuPanel.SetActive(true);
            resultText.text = isWinner ? "You win." : "You loose";
        }

        public void RestartGame()
        {
            menuPanel.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}