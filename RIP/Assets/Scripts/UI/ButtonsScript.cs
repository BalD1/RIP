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
            case "MainMenu":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void GameOverScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Restart":

                break;
            case "MainMenu":

                break;
        }
    }

    public void MainMenuScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Start the game":
                SceneManager.LoadScene("MainScene");
                break;
            case "Quit the game":
                GameManager.Instance.QuitGame();
                break;
        }
    }
}
