﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField] GameObject building;

    private void OnMouseEnter()
    {
        if (this.name != "DestoyButton")
        {
            UIManager.Instance.GetSelectedBuildingName(this.name);
        }
    }

    private void OnMouseExit()
    {
        if (this.name != "DestoyButton")
        {
            UIManager.Instance.GetSelectedBuildingName("");
        }
    }

    private void OnMouseDown()
    {
        if (this.isActiveAndEnabled)
        {
            if (this.name != "DestoyButton")
            {
                if (UIManager.Instance.SendCanPlaceBuilding())
                {
                    Vector3 holderPos = new Vector2();

                    holderPos = UIManager.Instance.SendActiveHolder().transform.position;
                    holderPos.z--;

                    Instantiate(building, holderPos, Quaternion.identity);
                    UIManager.Instance.GetBuildBubblesState(false);
                    UIManager.Instance.GetActiveHolder(null);
                }
            }
            else
            {
                Destroy(UIManager.Instance.SendActiveBuilding());
                UIManager.Instance.GetDestroyBubbleState(false);
                UIManager.Instance.GetActiveBuilding(null);
            }
        }
    }

}