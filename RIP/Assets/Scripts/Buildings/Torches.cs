using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torches : MonoBehaviour
{
    private bool isLighted;
    private bool isUsed;

    private void Update()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            this.isLighted = false;
            this.isUsed = false;
        }
        else if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && isUsed == false)
        {
            this.isLighted = true;
        }
        Debug.Log(isLighted);
    }

    public bool IsLighted()
    {
        return isLighted;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && this.isLighted)
        {
            this.isLighted = false;
            this.isUsed = true;
        }
    }

}
