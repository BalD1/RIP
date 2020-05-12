using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private float timer = 0;
    private bool Day = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 20.0f) // Valeur a changer pour changer le temps du cycle jour nuit
        {
            timer += Time.deltaTime;
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
