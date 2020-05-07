using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Text comptItems1;
    [SerializeField] private Text comptItems2;
    [SerializeField] private Text score;
    [SerializeField] private Image hpBar;

    [SerializeField] private Canvas buildBubbles;

    private int compTest;
    private float health;

    private bool bubblesState;

    private GameObject activeHolder;

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
        instance = this;
        this.ResetScriptable();
        health = playerValues.HpValue;
        buildBubbles.enabled = false;
    }

    void Update()
    {
        var rand = Random.Range(341, 789);
        if(Input.GetKey(KeyCode.L))
        {
            compTest+= rand;
        }

        if (hpBar != null && comptItems1 != null && comptItems2 != null && score != null)
        {
            hpBar.fillAmount = playerValues.HpValue / health;
            comptItems1.text = "            x " + playerValues.bonesCount.ToString() + System.Environment.NewLine + System.Environment.NewLine
                             + "            x " + playerValues.fleshCount.ToString();
            comptItems2.text = "            x " + playerValues.slimeCount.ToString() + System.Environment.NewLine + System.Environment.NewLine
                             + "            x " + playerValues.ectoplasmCount.ToString();
            score.text = "Score : " + compTest.ToString();
        }
    }

    private void ResetScriptable()          // A supprimer au build, sert à reset les valeurs du scriptable aux valeurs par défaut 
    {
        playerValues.HpValue = 10;
        playerValues.speed = 5;
        playerValues.fireBallDamages = 2;
        playerValues.fireBallLaunchSpeed = 7;
        playerValues.fireBallCooldown = 1.5f;
        playerValues.shovelDamages = 3;
        playerValues.shovelCooldown = 1;
        playerValues.fleshCount = 0;
        playerValues.bonesCount = 0;
        playerValues.slimeCount = 0;
        playerValues.ectoplasmCount = 0;
        playerValues.invincibleTime = 1;
    }

    // -------------------- Holders ------------------

    public void GetBuildBubblesState(bool state)
    {
        bubblesState = state;
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
    
}
