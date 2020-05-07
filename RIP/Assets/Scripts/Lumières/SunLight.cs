using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLight : MonoBehaviour
{
    private Light sunLight;

    void Start()
    {
        sunLight = this.GetComponent<Light>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            this.sunLight.intensity += 0.5f;
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            this.sunLight.intensity -= 0.5f;
        }
    }
}
