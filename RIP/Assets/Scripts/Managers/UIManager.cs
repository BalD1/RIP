using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Text fleshDisplay;
    [SerializeField] private Text boneDisplay;
    [SerializeField] private Text slimeDisplay;
    [SerializeField] private Text ectoplasmDisplay;
    [SerializeField] private Text score;
    [SerializeField] private Text level;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private Image fleshImage;
    [SerializeField] private Image boneImage;
    [SerializeField] private Image slimeImage;
    [SerializeField] private Image ectoplasmImage;

    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Canvas buildBubbles;
    [SerializeField] private Canvas destroyBubble;

    [SerializeField] private int automaticPauseTime;
    [SerializeField] private int flowerCost;
    [SerializeField] private int feedbackTextStopTime;

    [SerializeField] private Animator itemsHUDanimator;
    [SerializeField] private Animator feedbackTextAnimator;

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
    private bool upgradeBubbleDisplay;
    private bool destroyBubbleDisplay;

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
        notEnoughRessources = false;
        upgradeBubbleDisplay = false;
        feedbackText.enabled = false;
        destroyBubbleDisplay = false;
        refundFlag = false;
        itemsHUDanimator.SetBool("Show", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
        }
        this.AutomaticPause();

        var rand = Random.Range(341, 789);
        if(Input.GetKey(KeyCode.L))
        {
            compTest+= rand;
        }

        if (hpBar != null && fleshDisplay != null && boneDisplay != null && slimeDisplay != null && ectoplasmDisplay != null && score != null)
        {
            float fill = (float)playerValues.HpValue / (float)playerValues.maxHP;
            hpBar.fillAmount = fill;
            fill = (float)playerValues.xpAmount / (float)playerValues.xpNeeded;
            xpBar.fillAmount = fill;
            this.RessourcesDisplay();
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
        if (!this.buildBubbles.enabled && !this.destroyBubbleDisplay && !this.upgradeBubbleDisplay && !this.UnlockDisplay)
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
        else if (this.buildBubbles.enabled || this.upgradeBubbleDisplay || this.UnlockDisplay)
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
        else if (destroyBubbleDisplay && !refundFlag)
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

    public Image FleshDisplay
    {
        get
        {
            return fleshImage;
        }
    }

    public Image BoneDisplay
    {
        get
        {
            return boneImage;
        }
    }

    public Image SlimeDisplay
    {
        get
        {
            return slimeImage;
        }
    }

    public Image EctoplasmDisplay
    {
        get
        {
            return ectoplasmImage;
        }
    }

    public void ChangeLevelDisplay()
    {
        this.level.text = playerValues.level.ToString();
    }

    public void ShowItemsHUD()
    {
        if (itemsHUDanimator.GetBool("Show"))
        {
            itemsHUDanimator.SetBool("Show", false);
            itemsHUDanimator.SetBool("Hide", true);
        }
        else
        {
            itemsHUDanimator.SetBool("Show", true);
            itemsHUDanimator.SetBool("Hide", false);
        }
    }

    public bool HUDAnimatorBool
    {
        get
        {
            return itemsHUDanimator.GetBool("Show");
        }
        set
        {
            itemsHUDanimator.SetBool("Show", value);
            itemsHUDanimator.SetBool("Hide", !value);
        }
    }

    public bool WasHUDHidden { get; set; }

    public bool BuildDisplayActive { get; set; }

    public void DisplayFeedbackText(string text)
    {
        float waitTime = (GameManager.Instance.GetAnimationTimes(feedbackTextAnimator, "FeedbackText") * 2);
        if (feedbackTextAnimator.GetBool("Play") == false)
        {
            feedbackText.enabled = true;
            feedbackText.text = text;
            feedbackTextAnimator.SetBool("Play", true);
            StartCoroutine(WaitForAnimationEnd((waitTime / 2), ""));
        }
        else
        {
            StartCoroutine(WaitForAnimationEnd(waitTime, text));
        }
    }

    IEnumerator WaitForAnimationEnd(float time, string text)
    {
        yield return new WaitForSeconds(time);

        if (text == "")
        {
            feedbackTextAnimator.SetBool("Play", false);
        }
        else
        {
            DisplayFeedbackText(text);
        }
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
        this.gameOverScreen.SetActive(true);
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

    public Animator BubblesAnimator { get; set; }

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

    public bool UpgradeBubbleDisplay
    {
        get
        {
            return upgradeBubbleDisplay;
        }
        set
        {
            upgradeBubbleDisplay = value;
        }
    }
    public bool DestroyBubbleDisplay
    {
        get
        {
            return destroyBubbleDisplay;
        }
        set
        {
            destroyBubbleDisplay = value;
        }
    }

    public bool CanUpgrade { get; set; }
    public bool AddFlower { get; set; }
    public bool isBuildingMaxLevel { get; set; }
    public bool isBuildingFlowered { get; set; }
    
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

    public bool SendEnoughRessources()
    {
        return !notEnoughRessources;
    }

    public bool UnlockDisplay { get; set; }
    public bool CanUnlock
    {
        get
        {
            return !notEnoughRessources;
        }
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
