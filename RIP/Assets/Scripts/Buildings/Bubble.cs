using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{

    [SerializeField] GameObject building;
    [SerializeField] Animator bubblesAnimator;

    private Color originalColor;

    private Image image;

    private bool wasHUDHidden;
    

    private void Start()
    {
        image = this.GetComponent<Image>();
        originalColor = this.image.color;
        UIManager.Instance.BubblesAnimator = bubblesAnimator;
    }

    private void Update()
    {
        if (this.isActiveAndEnabled)
        {
            if (this.name == "FlowerButton")
            {
                if (UIManager.Instance.isBuildingFlowered)
                {
                    this.image.color = Color.gray;
                }
                else
                {
                    this.image.color = originalColor;
                }
            }

            if (this.name == "UpgradeButton")
            {
                if (UIManager.Instance.isBuildingMaxLevel)
                {
                    this.image.color = Color.gray;
                }
                else
                {
                    this.image.color = originalColor;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        GameManager.Instance.MouseIsOverSomething = true;

        if (this.name == "UpgradeButton" && !UIManager.Instance.isBuildingMaxLevel)
        {
            UIManager.Instance.UpgradeBubbleDisplay = true;
        }
        else if (this.name == "DestoyButton")
        {
            UIManager.Instance.DestroyBubbleDisplay = true;
        }
        else if (this.name != "DestoyButton")
        {
            UIManager.Instance.GetSelectedBuildingName(this.name);
        }
    }

    private void OnMouseExit()
    {
        GameManager.Instance.MouseIsOverSomething = false;

        if (UIManager.Instance.UpgradeBubbleDisplay)
        {
            UIManager.Instance.UpgradeBubbleDisplay = false;
        }
        else if (UIManager.Instance.DestroyBubbleDisplay)
        {
            UIManager.Instance.DestroyBubbleDisplay = false;
        }
        else if (this.name != "DestoyButton")
        {
            UIManager.Instance.GetSelectedBuildingName("");
        }
    }

    private void OnMouseDown()
    {
        if (this.gameObject != null && this.isActiveAndEnabled)
        {
            if (this.name == "DestoyButton")
            {
                Destroy(UIManager.Instance.SendActiveBuilding());
                UIManager.Instance.GetDestroyBubbleState(false);
                UIManager.Instance.GetActiveBuilding(null);
                if (UIManager.Instance.WasHUDHidden)
                {
                    UIManager.Instance.HUDAnimatorBool = false;
                }
            }
            else if (this.name == "UpgradeButton" && !UIManager.Instance.isBuildingMaxLevel)
            {
                if (UIManager.Instance.SendEnoughRessources())
                {
                    UIManager.Instance.CanUpgrade = true;
                }
            }
            else if (this.name == "FlowerButton" && !UIManager.Instance.isBuildingFlowered)
            {
                UIManager.Instance.AddFlower = true;
            }
            else
            {
                if (UIManager.Instance.SendCanPlaceBuilding())
                {
                    Vector3 holderPos = new Vector2();

                    if (UIManager.Instance.SendActiveHolder() != null)
                    {
                        holderPos = UIManager.Instance.SendActiveHolder().transform.position;
                        holderPos.z--;

                        Instantiate(building, holderPos, Quaternion.identity);
                        UIManager.Instance.GetBuildBubblesState(false);
                        UIManager.Instance.GetActiveHolder(null);
                        if (UIManager.Instance.WasHUDHidden)
                        {
                            UIManager.Instance.HUDAnimatorBool = false;
                        }
                    }
                }
            }
        }
    }

}
