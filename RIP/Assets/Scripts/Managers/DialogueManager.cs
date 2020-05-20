using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    [SerializeField] private Text dialogueText;
    [SerializeField] private PlayerValues playerValues;

    private static DialogueManager instance;
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Lol wtf l'instance du DialogueManager est null XD");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
            
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        EndDialogue = false;

        if (sentences.Count == 0)
        {
            this.EndDialogue = true;
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void DisplayQuestNeeds(int flesh, int bone, int slime, int ectoplasm)
    {
        this.dialogueText.text = "Flesh : " + flesh.ToString() + " / " + playerValues.fleshCount + Environment.NewLine
                               + "bone : " + bone.ToString() + " / " + playerValues.bonesCount + Environment.NewLine
                               + "slime : " + slime.ToString() + " / " + playerValues.slimeCount + Environment.NewLine
                               + "ectoplasm : " + ectoplasm.ToString() + " / " + playerValues.ectoplasmCount;

    }

    public bool EndDialogue { get; set; }


}
