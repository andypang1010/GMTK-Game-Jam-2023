using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    private string gameScene = "Game";

    public void StartButton()
    {
        SceneManager.LoadScene(gameScene);
    }
}
