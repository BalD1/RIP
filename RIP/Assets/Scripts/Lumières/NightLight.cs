using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{
    private Light nightLight; //QuentinLamy

    void Start()
    {
        nightLight = this.GetComponent<Light>();
    }

    void Update()
    {
        if(GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            this.nightLight.intensity = 0;
        }
        else
        {
            this.nightLight.intensity = 10;
        }
    }
}
