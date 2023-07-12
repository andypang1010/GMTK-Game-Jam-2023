using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameHUD, pauseMenu, lostScreen;
    public GameObject pauseButton, continueButton;
    public TMP_Text scoreText, healthText, timeText, waveText;

    void Update()
    {
        switch (GameManager.Instance.GetGameStates())
        {
            case GameStates.MainMenu:
                break;

            case GameStates.InGame:
                pauseMenu.SetActive(false);
                lostScreen.SetActive(false);
                gameHUD.SetActive(true);

                continueButton.SetActive(false);
                pauseButton.SetActive(true);
                scoreText.text = "Score: " + StatsManager.Instance.GetScore();
                healthText.text = " × " + StatsManager.Instance.GetHealth();

                break;

            case GameStates.Pause:
                lostScreen.SetActive(false);
                pauseMenu.SetActive(true);
                gameHUD.SetActive(true);

                continueButton.SetActive(true);
                pauseButton.SetActive(false);
                timeText.text = "You've survived: " + StatsManager.Instance.formattedTime;

                break;

            case GameStates.Lost:
                gameHUD.SetActive(false);
                pauseMenu.SetActive(false);
                lostScreen.SetActive(true);

                waveText.text = "YOU MADE IT TO WAVE " + LevelManager.Instance.GetWave();

                break;
        }
    }
}