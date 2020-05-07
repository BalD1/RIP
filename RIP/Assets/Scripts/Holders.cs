using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holders : MonoBehaviour
{

    public bool isUsed;

    void Start()
    {
        isUsed = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Tomb building = collision.gameObject.GetComponent<Tomb>();
        if (building != null && !GameManager.Instance.SendIsHolding())
        {
            this.isUsed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tomb building = collision.gameObject.GetComponent<Tomb>();
        if (building != null)
        {
            this.isUsed = false;
        }
    }



}
