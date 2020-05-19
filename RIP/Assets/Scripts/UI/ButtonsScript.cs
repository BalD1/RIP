using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{

    public void PauseScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Continue":
                GameManager.Instance.SetGameState(GameManager.GameState.InGame);
                break;
            case "Volume":

                break;
            case "Main Menu":

                break;
        }
    }

    public void GameOverScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Restart":

                break;
            case "Main Menu":

                break;
        }
    }
}
