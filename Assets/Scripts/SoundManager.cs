using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void Update()
    {
        switch(GameManager.Instance.GetGameStates()) {
            case GameStates.InGame:
                AudioListener.pause = false;
                break;
            case GameStates.Pause:
            case GameStates.Lost:
            case GameStates.MainMenu:
                AudioListener.pause = true;
                break;
        }
    }
}
