using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private bool tuto;

    private GameState currentState;
    private GameTime currentTime;

    private int damagesToPlayer;
    private int damagesToEnnemi;

    private Vector2 bubblesHolderPosition;

    private GameObject holdingBuilding;

    private bool isHolding;

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
        this.Tuto = tuto;
    }

    void Start()
    {
        this.currentTime = GameTime.Day;
        DayCount = 1;
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

    public GameState StateOfGame { get; set; }

    public enum GameTime
    {
        Day,
        Night,
    }

    public GameTime SendGameTime()
    {
        return this.currentTime;
    }

    public int DayCount { get; set; }

    public void SetGameTime(GameTime gameTime)
    {
        this.currentTime = gameTime;
        if (gameTime == GameTime.Day)
        {
            DayCount++;
        }
    }

    public bool Tuto { get; set; }

    // ------------------  Player Interactions ---------------

    public void DamagePlayer(int damages)
    {
        damagesToPlayer = damages;
    }

    public int SendDamages()
    {
        return damagesToPlayer;
    }

    public bool PlayerCanInteract { get; set; }

    public bool PlayerInteracted { get; set; }

    // ------------------  Ennemi Interactions ---------------

    public void DamageEnnemi(int damagesEnnemi)
    {
        damagesToEnnemi = damagesEnnemi;
    }

    public int SendDamagesEnnemi()
    {
        return damagesToEnnemi;
    }

    // ------------------- Buildings ------------------------

    public void GetIsHolding(bool hold)
    {
        isHolding = hold;
    }

    public bool SendIsHolding()
    {
        return isHolding;
    }

    public void GetBubblesHolderPosition(Vector2 position)
    {
        bubblesHolderPosition = position;
    }

    public Vector2 SendBubblesHolderPosition()
    {
        return bubblesHolderPosition;
    }

    //-------------- Quit Game ------------

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Has quit da game.");
    }
    
}
