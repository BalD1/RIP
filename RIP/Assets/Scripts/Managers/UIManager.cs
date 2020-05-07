using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Text comptItems1;
    [SerializeField] private Text comptItems2;
    [SerializeField] private Text score;
    [SerializeField] private Image hpBar;

    private int compTest;
    private float health;

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
    }

    void Update()
    {
        var rand = Random.Range(341, 789);
        if(Input.GetKey(KeyCode.L))
        {
            compTest+= rand;
        }

        hpBar.fillAmount = playerValues.HpValue / health;

        comptItems1.text = "            x " + playerValues.bonesCount.ToString() + System.Environment.NewLine + System.Environment.NewLine
                         + "            x " + playerValues.fleshCount.ToString();

        comptItems2.text = "            x " + playerValues.slimeCount.ToString() + System.Environment.NewLine + System.Environment.NewLine
                         + "            x " + playerValues.ectoplasmCount.ToString();

        score.text = "Score : " + compTest.ToString();
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
}
