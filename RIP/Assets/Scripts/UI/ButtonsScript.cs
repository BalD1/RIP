using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    public GameObject Controls;

    public void PauseScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Continue":
                GameManager.Instance.SetGameState(GameManager.GameState.InGame);
                Time.timeScale = 1;
                break;
            case "Volume":

                break;
            case "MainMenu":
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
        }
    }

    public void GameOverScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                GameManager.Instance.SetGameState(GameManager.GameState.InGame);
                Time.timeScale = 1;
                break;
            case "MainMenu":
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
        }
    }

    public void MainMenuScreenOnClickEvent(string button)
    {
        switch (button)
        {
            case "Show Controls":
                Controls.SetActive(true);
                break;
            case "Start the game":
                SceneManager.LoadScene("MainScene");
                Time.timeScale = 1;
                break;
            case "Quit the game":
                GameManager.Instance.QuitGame();
                break;
        }
    }

    public void TimeSpeed(string button)
    {
        switch (button)
        {
            case "NormalTime":
                Time.timeScale = 1;
                break;

            case "FastTime":
                Time.timeScale = 4;
                break;
        }
    }
}
