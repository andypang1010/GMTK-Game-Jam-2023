using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameStates gameState;

    public int frameRate = 60;

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
                Time.timeScale = 1f;
                SceneManager.LoadScene("Game");
                break;

            case GameStates.Pause:
                Time.timeScale = 0f;
                break;

            case GameStates.Lost:
                Time.timeScale = 0f;
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