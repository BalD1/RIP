using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    [SerializeField]
    private GameObject Ennemy;
    [SerializeField]
    private int neededHolders;
    
    private int holdersCount;

    private Vector2 buildingPos;

    private bool Spawned = false;
    private bool isSelected;

    private SpriteRenderer spriteRenderer;

    private Color originalColor;

    void Start()
    {
        buildingPos = this.transform.position;
        isSelected = false;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        originalColor = this.spriteRenderer.color;
    }
    
    void Update()
    {
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
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            if (!this.name.Contains("Ghost"))
            {
                if (UIManager.Instance.SendActiveHolder() == null)
                {
                    if (UIManager.Instance.SendDestroyBubbleState() == false)
                    {
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
                SpriteRenderer holderSprite = holder.GetComponent<SpriteRenderer>();
                if (this.holdersCount == this.neededHolders)
                {
                    holderSprite.color = Color.green;
                    UIManager.Instance.GetCanPlaceBuilding(true);
                }
                else
                {
                    holderSprite.color = Color.red;
                    UIManager.Instance.GetCanPlaceBuilding(false);
                }
            }
        }
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


}
