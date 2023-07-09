using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameHUD;
    public GameObject pauseMenu;
    public GameObject lostScreen;
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

                scoreText.text = "Score: " + StatsManager.Instance.GetScore();
                healthText.text = "♥ × " + StatsManager.Instance.GetHealth();

                break;

            case GameStates.Pause:
                gameHUD.SetActive(false);
                lostScreen.SetActive(false);
                pauseMenu.SetActive(true);

                timeText.text = StatsManager.Instance.formattedTime;

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