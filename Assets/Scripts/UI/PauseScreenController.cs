using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreenController : MonoBehaviour
    {
        public GameObject pauseScreen;
        public Text header;
        public Button resumeButton;
        public Button retryButton;
        public Button level1Button;
        public Button level2Button;
        public Button level3Button;

        private const string PAUSE_TEXT = "Pause";
        private const string DEAD_TEXT = "Game over";

        private GameManager _gameManager;

        public void ShowPause()
        {
            pauseScreen.SetActive(true);
            header.text = PAUSE_TEXT;
            resumeButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);
        }

        public void ShowDead()
        {
            pauseScreen.SetActive(true);
            header.text = DEAD_TEXT;
            resumeButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(true);
        }

        public void Hide()
        {
            pauseScreen.SetActive(false);
        }

        public void SetGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
            SetButtonHandlers();
        }

        private void SetButtonHandlers()
        {
            resumeButton.onClick.AddListener(() =>
            {
                pauseScreen.SetActive(false);
                _gameManager.SetGameState(GameState.Play);
            });

            retryButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

            level1Button.onClick.AddListener(() => _gameManager.SwitchScene(SceneEnum.DESERT));
            level2Button.onClick.AddListener(() => _gameManager.SwitchScene(SceneEnum.BUILDING));
            //level3Button.onClick.AddListener(() => SwitchScene(SceneEnum.DESERT));
        }
    }
}