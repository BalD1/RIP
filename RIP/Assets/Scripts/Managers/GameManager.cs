using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameState currentState;
    private GameTime currentTime;

    private int damagesToPlayer;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Lol wtf l'instance du GameManager est null XD");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.currentTime = GameTime.Day;
    }

    // -------------------  GameState & GameTime ----------------

    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
        GameOver
    }



    public void SetGameState(GameState newGameState)
    {
        this.currentState = newGameState;
    }

    public GameState SendGameState()
    {
        return this.currentState;
    }

    public enum GameTime
    {
        Day,
        Night,
    }

    public GameTime SendGameTime()
    {
        return this.currentTime;
    }

    public void SetGameTime(GameTime gameTime)
    {
        this.currentTime = gameTime;
    }

    // ------------------  Player Interactions ---------------

    public void DamagePlayer(int damages)
    {
        damagesToPlayer = damages;
    }

    public int SendDamages()
    {
        return damagesToPlayer;
    }
}
