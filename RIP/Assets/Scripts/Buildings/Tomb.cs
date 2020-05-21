using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    [SerializeField]
    private GameObject Ennemy;
    [SerializeField]
    private int neededHolders;
    [SerializeField]
    private PlayerValues playerValues;

    [SerializeField] private int fleshCost;
    [SerializeField] private int boneCost;
    [SerializeField] private int slimeCost;
    [SerializeField] private int ectoplasmCost;

    private int fleshUpgradeCost;
    private int boneUpgradeCost;
    private int slimeUpgradeCost;
    private int ectoplasmUpgradeCost;

    private int paidFlesh;
    private int paidBones;
    private int paidSlime;
    private int paidEctoplasm;

    private int holdersCount;
    private int buildingLevel;

    private Vector2 buildingPos;
    private Vector2 bubblesHolder;

    private Animator animator;

    private bool Spawned = false;
    private bool isSelected;
    private bool isMaxLevel;
    private bool isFlowered;

    private enum FlowerState
    {
        None,
        Flowered,
        Fanned,
    }
    private FlowerState flowerState;

    private SpriteRenderer spriteRenderer;

    private Color originalColor;

    void Start()
    {
        isMaxLevel = false;
        isFlowered = false;
        playerValues.flowerCount++;
        animator = this.GetComponent<Animator>();
        flowerState = FlowerState.None;
        buildingLevel = 1;
        Upgrade();
        bubblesHolder = GameManager.Instance.SendBubblesHolderPosition();
        buildingPos = this.transform.position;
        isSelected = false;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        originalColor = this.spriteRenderer.color;
        if (!this.name.Contains("Ghost"))
        {
            playerValues.fleshCount -= fleshCost;
            playerValues.bonesCount -= boneCost;
            playerValues.slimeCount -= slimeCost;
            playerValues.ectoplasmCount -= ectoplasmCost;

            paidFlesh = fleshCost;
            paidBones = boneCost;
            paidSlime = slimeCost;
            paidEctoplasm = ectoplasmCost;
        }
    }

    void Update()
    {
        if (UIManager.Instance.SendActiveBuilding() == this.gameObject && UIManager.Instance.SendDestroyBubbleState() == true)
        {
            UIManager.Instance.GetRefunds(fleshCost, boneCost, slimeCost, ectoplasmCost);
        }
            buildingPos = this.transform.position;

        if (!this.name.Contains("Ghost"))
        {
            if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && Spawned == false)
            {
                Instantiate(Ennemy, buildingPos, Quaternion.identity);
                Spawned = true;
            }

            if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
            {
                Spawned = false;
            }
        }

        this.Upgrade();
    }

    private void Upgrade()
    {
        int costMultiplier = new int();
        costMultiplier = Mathf.RoundToInt(buildingLevel * 2 / 1.5f);

        fleshUpgradeCost = fleshCost * costMultiplier;
        boneUpgradeCost = boneCost * costMultiplier;
        slimeUpgradeCost = slimeCost * costMultiplier;
        ectoplasmUpgradeCost = ectoplasmCost * costMultiplier;

        if (UIManager.Instance.UpgradeBubbleDisplay && UIManager.Instance.SendActiveBuilding() == this.gameObject)
        {
            SendCosts(fleshUpgradeCost, boneUpgradeCost, slimeUpgradeCost, ectoplasmUpgradeCost);
            if (UIManager.Instance.CanUpgrade)
            {
                this.buildingLevel++;
                playerValues.fleshCount -= fleshUpgradeCost;
                playerValues.bonesCount -= boneUpgradeCost;
                playerValues.slimeCount -= slimeUpgradeCost;
                playerValues.ectoplasmCount -= ectoplasmUpgradeCost;
                if (buildingLevel == 3)
                {
                    isMaxLevel = true;
                }
                this.ChangeSprite();
                UIManager.Instance.CanUpgrade = false;
            }
        }
        if (UIManager.Instance.AddFlower && playerValues.flowerCount > 0 && this.flowerState != FlowerState.Flowered 
            && UIManager.Instance.SendActiveBuilding() == this.gameObject)
        {
            this.flowerState = FlowerState.Flowered;
            this.ChangeSprite();
            isFlowered = true;
            playerValues.flowerCount--;
            UIManager.Instance.AddFlower = false;
        }
        if (UIManager.Instance.SendDestroyBubbleState() && UIManager.Instance.SendActiveBuilding() == this.gameObject)
        {
            UIManager.Instance.isBuildingMaxLevel = isMaxLevel;
            UIManager.Instance.isBuildingFlowered = isFlowered;
        }
    }

    private void ChangeSprite()
    {
        switch (this.buildingLevel)
        {
            case 1:
                animator.SetBool("Destroyed", true);
                break;
            case 2:
                animator.SetBool("Destroyed", false);
                animator.SetBool("Damaged", true);
                break;
            case 3:
                animator.SetBool("Damaged", false);
                animator.SetBool("Intact", true);
                break;
        }

        switch (this.flowerState)
        {
            case FlowerState.None:
                animator.SetBool("NoFlowers", true);
                break;
            case FlowerState.Flowered:
                animator.SetBool("NoFlowers", false);
                animator.SetBool("Flowered", true);
                break;
            case FlowerState.Fanned:
                animator.SetBool("Flowered", false);
                animator.SetBool("Fanned", true);
                break;

        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            if (!this.name.Contains("Ghost"))
            {
                if (UIManager.Instance.SendActiveHolder() == null && UIManager.Instance.UnlockDisplay == false)
                {
                    if (UIManager.Instance.SendDestroyBubbleState() == false)
                    {
                        GameManager.Instance.GetBubblesHolderPosition(bubblesHolder);
                        UIManager.Instance.GetDestroyBubbleState(true);
                        UIManager.Instance.GetActiveBuilding(this.gameObject);
                        this.spriteRenderer.color = Color.gray;
                        isSelected = true;
                    }
                    else if (UIManager.Instance.SendActiveBuilding() == this.gameObject)
                    {
                        UIManager.Instance.GetDestroyBubbleState(false);
                        UIManager.Instance.GetActiveBuilding(null);
                        this.spriteRenderer.color = originalColor;
                        UIManager.Instance.isBuildingMaxLevel = false;
                        UIManager.Instance.isBuildingFlowered = false;
                        isSelected = false;
                    }
                }
                else
                {

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.name.Contains("Ghost"))
        {
            Holders holder = collision.GetComponent<Holders>();
            if (holder != null && !holder.IsUsed())
            {
                holdersCount++;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.name.Contains("Ghost"))
        {
            Holders holder = collision.GetComponent<Holders>();
            if (holder != null)
            {
                this.SendCosts(fleshCost, boneCost, slimeCost, ectoplasmCost);
                SpriteRenderer holderSprite = holder.GetComponent<SpriteRenderer>();
                if (this.holdersCount == this.neededHolders)
                {
                    holderSprite.color = Color.green;
                    UIManager.Instance.BuildingHaveEnoughHolders(true);
                }
                else
                {
                    holderSprite.color = Color.red;
                    UIManager.Instance.BuildingHaveEnoughHolders(false);
                }
            }
        }
    }

    private void SendCosts(int flesh, int bone, int slime, int ectoplasm)
    {
        UIManager.Instance.GetBuildingsCosts(flesh, bone, slime, ectoplasm);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.name.Contains("Ghost"))
        {
            Holders holder = collision.GetComponent<Holders>();
            if (holder != null)
            {
                SpriteRenderer holderSprite = holder.GetComponent<SpriteRenderer>();
                if (holder != null && !holder.IsUsed() && !holder.IsActive())
                {
                    holdersCount--;
                    holderSprite.color = holder.OriginalColor();
                }
                else if (holder != null && !holder.IsUsed() && holder.IsActive())
                {
                    holdersCount--;
                    holderSprite.color = Color.grey;
                }
                else if (holder != null)
                {
                    holderSprite.color = holder.OriginalColor();
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (UIManager.Instance.SendActiveBuilding() == this.gameObject && UIManager.Instance.SendDestroyBubbleState())
        {
            UIManager.Instance.GetDestroyBubbleState(false);
            UIManager.Instance.GetActiveBuilding(null);
            this.spriteRenderer.color = originalColor;
        }
    }

    private void OnDestroy()
    {
        playerValues.fleshCount += paidFlesh;
        playerValues.bonesCount += paidBones;
        playerValues.slimeCount += paidSlime;
        playerValues.ectoplasmCount += paidEctoplasm;
        UIManager.Instance.DestroyBubbleDisplay = false;
    }


}