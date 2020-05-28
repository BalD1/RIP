using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horloge : MonoBehaviour
{
    Transform transform;

    private int rotate;

    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            rotate += 10;
            transform.rotation = Quaternion.Euler(0,0,-rotate);
        }
    }
}
