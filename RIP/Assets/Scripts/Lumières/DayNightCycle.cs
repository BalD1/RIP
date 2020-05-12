using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private Light sunLight;
    private float timer = 0;
    private bool Day = true;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 15.0f)
        {
            if (Day == true)
            {
                    this.sunLight.intensity -= 0.0025f;
            }
            else
            {
                    this.sunLight.intensity += 0.0025f;
            }
        }
        if (timer <= 20.0f) // Valeur a changer pour changer le temps du cycle jour nuit
        {
            timer += Time.deltaTime;
            if(Day == true)
            {
                if(this.sunLight.intensity < 2)
                {
                    this.sunLight.intensity += 0.001f;
                }
            }
            else
            {
                if(this.sunLight.intensity > 0.75f)
                {
                    this.sunLight.intensity -= 0.001f;
                }
            }
        }
        else if (Day == true)
        {
            GameManager.Instance.SetGameTime(GameManager.GameTime.Night);
            timer = 0;
            Day = false;

        }
        else if (Day == false)
        {
            GameManager.Instance.SetGameTime(GameManager.GameTime.Day);
            timer = 0;
            Day = true;
        }
    }
}
