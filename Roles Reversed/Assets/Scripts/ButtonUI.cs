using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void GameButton()
    {
        GameManager.Instance.SetGameStates(GameStates.InGame);
    }

    public void PauseButton()
    {
        GameManager.Instance.SetGameStates(GameStates.Pause);
    }

    public void MainMenuButton()
    {
        GameManager.Instance.SetGameStates(GameStates.MainMenu);
    }
}
