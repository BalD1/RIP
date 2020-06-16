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
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScore;
    private Text textToDestroy;

    [SerializeField] private float baseSpeedCount;
    [SerializeField] private float moveDelayTime;
    
    [SerializeField] private int buildingsMultiplier;
    [SerializeField] private int questsMultiplier;
    [SerializeField] private int nightsMultiplier;
    [SerializeField] private int levelMultiplier;
    [SerializeField] private int totalKillsMultiplier;
    private int killsMultiplier;
    private int score;
    private int higherScore;

    [SerializeField] private Transform scoreTransform;

    private GameManager.PlayerStats playerStats;

    private List<Text> textToScoreList;

    private bool flag;
    private bool canGetScore;
    private bool canMove;

    private void Start()
    {
        higherScore = PlayerPrefs.GetInt("highscore");
        bestScore.text = higherScore.ToString();
        textToScoreList = new List<Text>();
        flag = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameOverScreenIsShowing && !flag)
        {
            playerStats = GameManager.Instance.currentStats;
            KillsMultiplier();
            IncreaseCount(0, playerStats.totalKills, baseSpeedCount, killsCount, killsMultiplier);
            IncreaseCount(0, playerStats.buildingsCount, baseSpeedCount, buildingsCount, buildingsMultiplier);
            IncreaseCount(0, playerStats.questsCount, baseSpeedCount, questsCount, questsMultiplier);
            IncreaseCount(0, playerStats.nightsCount, baseSpeedCount, nightsCount, nightsMultiplier);
            IncreaseCount(0, playerStats.playerlevel, baseSpeedCount, playerLevel, levelMultiplier);
            flag = true;
        }

        if (textToScoreList.Count > 0)
        {
            MoveTexts();
        }
        if (textToDestroy != null)
        {
            textToScoreList.Remove(textToDestroy);
            Destroy(textToDestroy);
            textToDestroy = null;
        }
    }

    private void KillsMultiplier()
    {
        List<int> kills = new List<int>();
        kills.Add(playerStats.zombieKills);
        kills.Add(playerStats.skeletonKills);
        kills.Add(playerStats.slimeKills);
        kills.Add(playerStats.ghostsKills);
        foreach(int killCount in kills)
        {
            killsMultiplier += killCount * (int)((playerStats.totalKills / 1.99f) - killCount);
        }

        killsMultiplier = killsMultiplier / playerStats.totalKills * totalKillsMultiplier ;
    }

    private void IncreaseCount(int index, int goToCount, float speedCount, Text textToIncrease, int multiplier)
    {
        textToIncrease.text = index.ToString();
        if (index < goToCount)
        {
            index++;
            canGetScore = false;
            StartCoroutine(Increase(index, goToCount, speedCount, textToIncrease, multiplier));
        }
        else
        {
            canGetScore = true;
            Text textToScore = Instantiate(textToIncrease, textToIncrease.transform);
            textToScore.text = Mathf.Clamp((goToCount * multiplier), 0, Mathf.Infinity).ToString();
            Debug.Log(textToScore.text);
            textToScore.color = Color.red;
            textToScoreList.Add(textToScore);
        }
    }

    IEnumerator Increase(int index, int goToCount, float speedCount, Text textToIncrease, int multiplier)
    {
        yield return new WaitForSecondsRealtime(speedCount);
        speedCount = (float)(speedCount / 1.2);
        IncreaseCount(index, goToCount, speedCount, textToIncrease, multiplier);
    }

    private void CalculateScore(int increase, Text receivedText)
    {
        score += increase;

        scoreText.text = score.ToString();
        textToScoreList.Remove(receivedText);
        Destroy(receivedText);
        if (score > higherScore)
        {
            higherScore = score;
            PlayerPrefs.SetInt("highscore", higherScore);
            bestScore.text = higherScore.ToString();
        }
    }

    private void MoveTexts()
    {
        foreach(Text text in textToScoreList)
        {
            Vector3 positionDiffenrece = ((text.rectTransform.position - scoreTransform.position) * 0.01f);
                text.rectTransform.position -= positionDiffenrece;
            if (text.rectTransform.position.x < scoreTransform.position.x * 1.1 &&
                text.rectTransform.position.y < scoreTransform.position.y * 1.1)
            {
                CalculateScore(int.Parse(text.text), text);
                return;
            }
        }
    }
}
