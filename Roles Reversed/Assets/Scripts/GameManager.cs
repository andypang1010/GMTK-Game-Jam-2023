using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public int frameRate = 60;
    public GameStates gameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Application.targetFrameRate = frameRate;
        }
    }

    public GameStates GetGameStates()
    {
        return gameState;
    }

    public void SetGameStates(GameStates states)
    {
        GameStates lastGameState = gameState;
        gameState = states;

        switch (gameState) {
            case GameStates.MainMenu:
                SceneManager.LoadScene("Menu");
                break;

            case GameStates.InGame:
                switch (lastGameState) {
                    case GameStates.Pause:
                        Time.timeScale = 1f;
                        break;
                    default:
                        SceneManager.LoadScene("Game");
                        break;
                }
                break;

            case GameStates.Pause:
                Time.timeScale = 0f;
                break;

            case GameStates.Lost:
                break;

        }
    }

}

public enum GameStates
{
    MainMenu,
    InGame,
    Pause,
    Lost
}