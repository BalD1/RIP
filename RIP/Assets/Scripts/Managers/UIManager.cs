using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Text fleshDisplay;
    [SerializeField] private Text boneDisplay;
    [SerializeField] private Text slimeDisplay;
    [SerializeField] private Text ectoplasmDisplay;
    [SerializeField] private Text score;
    [SerializeField] private Image hpBar;

    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Canvas buildBubbles;
    [SerializeField] private Canvas destroyBubble;
    [SerializeField] private Canvas unlockBubble;

    [SerializeField] private int automaticPauseTime;
    private int compTest;
    private int fleshCost;
    private int fleshRefund;
    private int boneCost;
    private int boneRefund;
    private int slimeCost;
    private int slimeRefund;
    private int ectoplasmCost;
    private int ectoplasmRefund;

    private float health;
    private float automaticPauseTimer;

    private bool costDisplayFlag;
    private bool bubblesState;
    private bool destroyBubblesState;
    private bool unlockBubbleState;
    private bool notEnoughRessources;
    private bool canPlace;
    private bool refundFlag;
    private bool canUnlock;

    private Color originalDisplayTextColor;

    private GameObject activeHolder;
    private GameObject activeBuilding;

    private string selectedBuilding;

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Ca par exemple, l'instance du UI Manager est nulle :o");
            }
            return instance;
        }
    }

    private void Awake()
    {
        automaticPauseTimer = automaticPauseTime;
        originalDisplayTextColor = fleshDisplay.color;
        instance = this;
        health = playerValues.HpValue;
        buildBubbles.enabled = false;
        destroyBubble.enabled = false;
        unlockBubble.enabled = false;
        notEnoughRessources = false;
        refundFlag = false;
    }

    void Update()
    {
        this.AutomaticPause();

        var rand = Random.Range(341, 789);
        if(Input.GetKey(KeyCode.L))
        {
            compTest+= rand;
        }

        if (hpBar != null && fleshDisplay != null && boneDisplay != null && slimeDisplay != null && ectoplasmDisplay != null && score != null)
        {
            this.RessourcesDisplay();
            hpBar.fillAmount = playerValues.HpValue / health;
            score.text = "Score : " + compTest.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.SendGameState() == GameManager.GameState.InGame)
            {
                this.SetPause();
            }
            else
            {
                this.RemovePause();
            }
        }
        if (GameManager.Instance.SendGameState() == GameManager.GameState.Pause)
        {
            this.SetPause();
        }
        if (GameManager.Instance.SendGameState() == GameManager.GameState.InGame && PauseScreen.activeSelf)
        {
            this.RemovePause();
        }
        if (GameManager.Instance.SendGameState() == GameManager.GameState.GameOver)
        {
            this.GameOver();
        }
    }

    private void AutomaticPause()
    {
        if (Input.anyKeyDown)
        {
            automaticPauseTimer = automaticPauseTime;
        }
        if (automaticPauseTimer == 0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Pause);
        }
        if (GameManager.Instance.SendGameState() == GameManager.GameState.InGame)
        {
            automaticPauseTimer = Mathf.Clamp(automaticPauseTimer - Time.deltaTime, 0, automaticPauseTimer);
        }
    }

    // -------------------- HUD ----------------------

    private void RessourcesDisplay()
    {
        if (!this.buildBubbles.enabled && !this.destroyBubble.enabled && !this.unlockBubble.enabled)
        {
            this.fleshDisplay.text = "x " + playerValues.fleshCount.ToString();
            this.boneDisplay.text = "x " + playerValues.bonesCount.ToString();
            this.slimeDisplay.text = "x " + playerValues.slimeCount.ToString();
            this.ectoplasmDisplay.text = "x " + playerValues.ectoplasmCount.ToString();

            if (fleshCost != 0 || boneCost != 0 || slimeCost != 0 || ectoplasmCost != 0 ||
                fleshRefund != 0 || boneRefund != 0 || slimeRefund != 0 || ectoplasmRefund != 0)
            {
                ResetDisplay();
            }

        }
        else if (this.buildBubbles.enabled || this.unlockBubble.enabled)
        {
            this.fleshDisplay.text = "x " + playerValues.fleshCount + " / " + fleshCost;
            ColorDisplayText(fleshDisplay, playerValues.fleshCount, fleshCost);

            this.boneDisplay.text = "x " + playerValues.bonesCount + " / " + boneCost;
            ColorDisplayText(boneDisplay, playerValues.bonesCount, boneCost);

            this.slimeDisplay.text = "x " + playerValues.slimeCount + " / " + slimeCost;
            ColorDisplayText(slimeDisplay, playerValues.slimeCount, slimeCost);

            this.ectoplasmDisplay.text = "x " + playerValues.ectoplasmCount + " / " + ectoplasmCost;
            ColorDisplayText(ectoplasmDisplay, playerValues.ectoplasmCount, ectoplasmCost);
        }
        else if (this.destroyBubble.enabled && !refundFlag)
        {
            if(fleshRefund != 0)
            {
                this.fleshDisplay.text += " + " + fleshRefund;
                this.fleshDisplay.color = Color.green;
                refundFlag = true;
            }
            if (boneRefund != 0)
            {
                this.boneDisplay.text += " + " + boneRefund;
                this.boneDisplay.color = Color.green;
                refundFlag = true;
            }
            if (slimeRefund != 0)
            {
                this.slimeDisplay.text += " + " + slimeRefund;
                this.slimeDisplay.color = Color.green;
                refundFlag = true;
            }
            if (ectoplasmRefund != 0)
            {
                this.ectoplasmDisplay.text += " + " + ectoplasmRefund;
                this.ectoplasmDisplay.color = Color.green;
                refundFlag = true;
            }
        }
    }

    private void ColorDisplayText(Text display, int count, int cost)
    {
        if (cost == 0)
        {
            display.color = originalDisplayTextColor;
        }
        else if (cost > count)
        {
            display.color = Color.red;
            notEnoughRessources = true;
        }
        else if (cost <= count)
        {
            if (display.color == Color.red)
            {
                notEnoughRessources = false;
            }
            display.color = Color.green;
        }
    }

    private void ResetDisplay()
    {
        fleshDisplay.color = originalDisplayTextColor;
        fleshCost = 0;
        fleshRefund = 0;

        boneDisplay.color = originalDisplayTextColor;
        boneCost = 0;
        boneRefund = 0;

        slimeDisplay.color = originalDisplayTextColor;
        slimeCost = 0;
        slimeRefund = 0;

        ectoplasmDisplay.color = originalDisplayTextColor;
        ectoplasmCost = 0;
        ectoplasmRefund = 0;

        notEnoughRessources = false;
        refundFlag = false;
    }


    // -------------------- Screens ------------------

    private void SetPause()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Pause);
        this.HUD.SetActive(false);
        this.PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void RemovePause()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.InGame);
        this.HUD.SetActive(true);
        this.PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    private void GameOver()
    {
        this.HUD.SetActive(false);
        this.gameOverScreen.SetActive(false);
        Time.timeScale = 0;
    }

    // -------------------- Holders ------------------
    
    public void GetBuildBubblesState(bool state)
    {
        bubblesState = state;
        buildBubbles.transform.position = GameManager.Instance.SendBubblesHolderPosition();
        buildBubbles.enabled = bubblesState;
    }

    public bool SendBuildBubblesState()
    {
        return bubblesState;
    }

    public void GetActiveHolder(GameObject holder)
    {
        activeHolder = holder;
    }

    public GameObject SendActiveHolder()
    {
        return activeHolder;
    }
    
    public void GetSelectedBuildingName(string building)
    {
        selectedBuilding = building;
    }

    public string SendSelectedBuildingName()
    {
        return selectedBuilding;
    }

    public void BuildingHaveEnoughHolders(bool state)
    {
        canPlace = state;
    }

    public bool SendCanPlaceBuilding()
    {
        if (canPlace && !notEnoughRessources)
        {
            return true;
        }
        return false;
    }

    // ------------------- Buildings ------------------
    
    public void GetActiveBuilding(GameObject building)
    {
        this.activeBuilding = building;
    }

    public GameObject SendActiveBuilding()
    {
        return this.activeBuilding;
    }

    public void GetDestroyBubbleState(bool state)
    {
        destroyBubble.transform.position = GameManager.Instance.SendBubblesHolderPosition();
        this.destroyBubblesState = state;
        destroyBubble.enabled = destroyBubblesState;
    }

    public bool SendDestroyBubbleState()
    {
        return this.destroyBubblesState;
    }

    public void GetUnlockBubbleState(bool state)
    {
        unlockBubble.transform.position = GameManager.Instance.SendBubblesHolderPosition();
        this.unlockBubbleState = state;
        unlockBubble.enabled = unlockBubbleState;
    }

    public bool SendEnoughRessources()
    {
        return !notEnoughRessources;
    }

    public bool SendUnlockBubbleState()
    {
        return this.unlockBubbleState;
    }

    public void GetCanUnlock(bool state)
    {
        this.canUnlock = state;
    }

    public bool SendCanUnlock()
    {
        return this.canUnlock;
    }

    public void GetBuildingsCosts(int getFleshCost, int getBoneCost, int getSlimeCost, int getEctoplasmCost)
    {
        this.fleshCost = getFleshCost;
        this.boneCost = getBoneCost;
        this.slimeCost = getSlimeCost;
        this.ectoplasmCost = getEctoplasmCost;
    }

    public void GetRefunds(int getFleshRefunds, int getBoneRefunds, int getSlimeRefund, int getEctoplasmRefund)
    {
        this.fleshRefund = getFleshRefunds;
        this.boneRefund = getBoneRefunds;
        this.slimeRefund = getSlimeRefund;
        this.ectoplasmRefund = getEctoplasmRefund;
    }
}
