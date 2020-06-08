using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private Text killsCount;
    [SerializeField] private Text buildingsCount;
    [SerializeField] private Text questsCount;
    [SerializeField] private Text nightsCount;
    [SerializeField] private Text playerLevel;
    [SerializeField] private Text score;
    [SerializeField] private Text bestScore;

    private GameManager.PlayerStats playerStats;

    private bool flag;

    private void Start()
    {
        flag = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameOverScreenIsShowing && !flag)
        {
            playerStats = GameManager.Instance.currentStats;
            IncreaseCount(0, playerStats.kills, killsCount);
            IncreaseCount(0, playerStats.buildingsCount, buildingsCount);
            IncreaseCount(0, playerStats.questsCount, questsCount);
            IncreaseCount(0, playerStats.nightsCount, nightsCount);
            IncreaseCount(0, playerStats.playerlevel, playerLevel);
            flag = true;
        }
    }

    private void IncreaseCount(int index, int goToCount, Text textToIncrease)
    {
        textToIncrease.text = index.ToString();
        if (index < goToCount)
        {
            index++;
            StartCoroutine(Increase(1, index, goToCount, textToIncrease));
        }
    }

    IEnumerator Increase(float time, int index, int goToCount, Text textToIncrease)
    {
        yield return new WaitForSecondsRealtime(time);

        IncreaseCount(index, goToCount, textToIncrease);
    }
}
