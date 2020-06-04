using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldersGroup : MonoBehaviour
{

    [SerializeField] private bool isLocked;
    [SerializeField] private GameObject lockDown;
    [SerializeField] private GameObject unlockSprite;

    [SerializeField] private int fleshCost;
    [SerializeField] private int boneCost;
    [SerializeField] private int slimeCost;
    [SerializeField] private int ectoplasmCost;

    [SerializeField] private PlayerValues playerValues;

    private void Awake()
    {
        this.gameObject.SetActive(isLocked);
    }

    private void OnMouseEnter()
    {
        GameManager.Instance.MouseIsOverSomething = true;
    }

    private void OnMouseOver()
    {
        UIManager.Instance.UnlockDisplay = true;
        UIManager.Instance.GetBuildingsCosts(fleshCost, boneCost, slimeCost, ectoplasmCost);
        if (UIManager.Instance.CanUnlock && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            unlockSprite.SetActive(true);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            unlockSprite.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        GameManager.Instance.MouseIsOverSomething = false;
        UIManager.Instance.UnlockDisplay = false;
        unlockSprite.SetActive(false);
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseDown()
    {
        if (UIManager.Instance.CanUnlock)
        {
            playerValues.fleshCount -= this.fleshCost;
            playerValues.bonesCount -= this.boneCost;
            playerValues.slimeCount -= this.slimeCost;
            playerValues.ectoplasmCount -= this.ectoplasmCost;
            this.gameObject.SetActive(false);
            UIManager.Instance.UnlockDisplay = false;
        }
    }

}
