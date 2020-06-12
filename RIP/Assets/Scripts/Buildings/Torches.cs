using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torches : MonoBehaviour
{
    [SerializeField] private GameObject lightPoint;

    private bool isLighted;
    private bool isUsed;

    private void Update()
    {
        lightPoint.SetActive(isLighted);

        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            this.isLighted = false;
            this.isUsed = false;
        }
        else if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && isUsed == false)
        {
            this.isLighted = true;
        }
    }

    public bool IsLighted()
    {
        if (this.isLighted && GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            this.isLighted = false;
            this.isUsed = true;
            return true;
        }
        return isLighted;
    }
    

}
