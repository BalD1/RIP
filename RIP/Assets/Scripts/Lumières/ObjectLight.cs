using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;


public class ObjectLight : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D Light;

    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && Light.intensity > 0)
        {
            Light.intensity -= 0.0040f;
        }
        else if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && Light.intensity < 1)
        {
            Light.intensity += 0.0040f;
        }
    }
}
