﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crow : MonoBehaviour
{

    [SerializeField] private PlayerValues playerValues;

    [SerializeField] private Dialogue tutoDialogues;
    [SerializeField] private Dialogue quest;
    [SerializeField] private Canvas dialogueBox;

    [SerializeField] private int maxRessourcesForQuest;
    [SerializeField] private int minRessourcesForQuest;
    [SerializeField] private int questChances;

    private bool haveAQuest;
    private bool dayFlag;
    private bool firstTutoDisplay;
    private bool displayQuestNeeds;
    private bool tuto;
    private bool playerInteractedOnThis;
    private bool playerIsOnThis;

    private int neededFlesh;
    private int neededBone;
    private int neededSlime;
    private int neededEctoplasm;
    private int experienceRewardMultiplier;
    private List<int> questRessources = new List<int>();

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        dialogueBox.gameObject.SetActive(false);
        firstTutoDisplay = true;
        displayQuestNeeds = false;
        this.tuto = GameManager.Instance.Tuto;
        this.haveAQuest = GameManager.Instance.Tuto;
        this.animator = this.gameObject.GetComponent<Animator>();
        if (tuto)
        {
            dayFlag = false;
        }
        else
        {
            dayFlag = true;
        }
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
        {
            this.spriteRenderer.flipX = true;
        }
        else
        {
            this.spriteRenderer.flipX = false;
        }

        animator.SetBool("QuestAvailable", haveAQuest);

        if (tuto && GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            tuto = false;
            haveAQuest = false;
        }
        if (tuto && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && playerIsOnThis)
        {
            if (!GameManager.Instance.PlayerCanInteract || firstTutoDisplay)
            {
                this.dialogueBox.gameObject.SetActive(false);
            }
            else
            {
                this.dialogueBox.gameObject.SetActive(true);
            }
            if (playerInteractedOnThis)
            {
                if (firstTutoDisplay)
                {
                    DialogueManager.Instance.StartDialogue(tutoDialogues);
                    firstTutoDisplay = false;
                }
                else
                {
                    DialogueManager.Instance.DisplayNextDialogue();
                }
                playerInteractedOnThis = false;
            }
            if (DialogueManager.Instance.EndDialogue)
            {
                this.dialogueBox.gameObject.SetActive(false);
                this.tuto = false;
                GameManager.Instance.Tuto = tuto;
                playerInteractedOnThis = false;
                this.haveAQuest = false;
            }

        }

        if (dayFlag && !haveAQuest && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            if (Random.Range(0, questChances) == 0)
            {
                haveAQuest = true;

                neededFlesh = Random.Range(minRessourcesForQuest, maxRessourcesForQuest);
                neededBone = Random.Range(minRessourcesForQuest, maxRessourcesForQuest);
                neededSlime = Random.Range(minRessourcesForQuest, maxRessourcesForQuest);
                neededEctoplasm = Random.Range(minRessourcesForQuest, maxRessourcesForQuest);

                experienceRewardMultiplier = Mathf.RoundToInt((neededFlesh + neededBone + neededSlime + neededEctoplasm) / 4);
            }
            dayFlag = false;
        }

        if (haveAQuest && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && !tuto && playerIsOnThis)
        {
            if (!displayQuestNeeds)
            {
                if (GameManager.Instance.PlayerCanInteract)
                {
                    this.dialogueBox.gameObject.SetActive(true);
                }
                else
                {
                    this.dialogueBox.gameObject.SetActive(false);
                }
                DialogueManager.Instance.StartDialogue(quest);

                if (GameManager.Instance.PlayerInteracted)
                {
                    displayQuestNeeds = true;
                    playerInteractedOnThis = false;
                }
            }
            else
            {
                DialogueManager.Instance.DisplayQuestNeeds(neededFlesh, neededBone, neededSlime, neededEctoplasm);
                if (GameManager.Instance.PlayerCanInteract)
                {
                    this.dialogueBox.gameObject.SetActive(true);
                    if (GameManager.Instance.PlayerInteracted)
                    {
                        playerInteractedOnThis = false;
                        PayRessources(ref playerValues.fleshCount, ref neededFlesh);
                        PayRessources(ref playerValues.bonesCount, ref neededBone);
                        PayRessources(ref playerValues.slimeCount, ref neededSlime);
                        PayRessources(ref playerValues.ectoplasmCount, ref neededEctoplasm);
                        CheckIfQuestCompleted();
                    }
                }
                else
                {
                    this.dialogueBox.gameObject.SetActive(false);
                }
            }

        }

        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            this.dialogueBox.gameObject.SetActive(false);
            dayFlag = true;
            this.animator.SetBool("QuestAvailable", false);
        }
        else
        {
            animator.SetBool("QuestAvailable", haveAQuest);
        }

    }

    private void PayRessources(ref int amount,ref int price)
    {
        if (price > amount)
        {
            price -= amount;
            amount = 0;
        }
        else
        {
            amount -= price;
            price = 0;

        }
    }

    private void CheckIfQuestCompleted()
    {
        questRessources.Add(neededFlesh);
        questRessources.Add(neededBone);
        questRessources.Add(neededSlime);
        questRessources.Add(neededEctoplasm);
        foreach (int price in questRessources)
        {
            if (price > 0)
            {
                questRessources.Clear();
                return;
            }
        }
        questRessources.Clear();
        Debug.Log("Quest Completed");
        dialogueBox.gameObject.SetActive(false);
        displayQuestNeeds = false;
        haveAQuest = false;
        GameManager.Instance.PlayerInteracted = false;
        playerInteractedOnThis = false;
    }

    private void RewardPlayer()
    {
        playerValues.xpAmount += playerValues.level * 100 * experienceRewardMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            GameManager.Instance.PlayerCanInteract = true;
            this.playerIsOnThis = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            GameManager.Instance.PlayerCanInteract = false;
        }
        else if (player != null)
        {
            if (GameManager.Instance.PlayerInteracted)
            {
                playerInteractedOnThis = true;
                GameManager.Instance.PlayerInteracted = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GameManager.Instance.PlayerCanInteract = false;
            this.playerIsOnThis = false;
            this.dialogueBox.gameObject.SetActive(false);
        }
    }
}