using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    public string[] Sounds = new string[]
    {
        "DeathZombie",
        "DeathSkeleton",
        "DeathSlime",
        "DeathGhost",
    };

    [SerializeField]
    private List<AudioClip> audioArray;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("le sound manager est null :c");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Play (string name)
    {
        for(int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                source.clip = audioArray[i];
                source.Play();
            }
        }
    }
}
