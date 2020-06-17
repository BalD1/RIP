using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour //QuentinLamy
{
    [SerializeField]
    private AudioSource source;

    private bool musicFlag = false;


    public string[] Sounds = new string[]
    {
        "DeathZombie",
        "DeathSkeleton",
        "DeathSlime",
        "DeathGhost",
        "CreatingFire",
        "FireImpact",
        "PickUp",
        "Building",
        "Destroying",
        "ShovelHit",
        "Bone",
        "Musique",
        "MusiqueMenu",
        "Points",
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

    private void Update()
    {
            PlayBGMusic();     
        if(!source.isPlaying)
        {
            musicFlag = false;
        }
    }

    public void Play (string name)
    {
        for(int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                source.PlayOneShot(audioArray[i]);
            }
        }
    }

    public void PlayBGMusic()
    {
        if(musicFlag == false)
        {
            musicFlag = true;
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                source.PlayOneShot(audioArray[11]);
            }
            else
            {
                source.PlayOneShot(audioArray[12]);
            }
        }

    }
}
