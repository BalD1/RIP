using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horloge : MonoBehaviour
{
    Transform transform;

    private float rotate;

    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        rotate = DayNightCycle.dayNightTimer * -7.2f - 240f;

        transform.rotation = Quaternion.Euler(0,0,rotate);
    }
}
