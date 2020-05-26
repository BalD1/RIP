using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNightCycle : MonoBehaviour
{
    private Light sunLight;
    private float timer = 0;
    private bool Day = true;
    public UnityEngine.Experimental.Rendering.Universal.Light2D SUN;

    // Start is called before the first frame update
    void Start()
    {
        SUN = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 44.0f)
        {
            if (Day == true)
            {
                    SUN.intensity -= 0.0020f;
            }
            else
            {
                SUN.intensity += 0.0020f;
            }
        }
        if (timer <= 50.0f) // Valeur a changer pour changer le temps du cycle jour nuit
        {
            timer += Time.deltaTime;
            if(Day == true)
            {
                if(SUN.intensity < 0.9f)
                {
                    SUN.intensity += 0.001f;
                }
            }
            else
            {
                if(SUN.intensity > 0.05f)
                {
                    SUN.intensity -= 0.001f;
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
