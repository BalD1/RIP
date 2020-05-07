using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    public GameObject Ennemy;
    public int neededHolders;
    private float holdersCount;

    private Vector2 buildingPos;
    private Vector2 placePointPos;
    private Vector2 holderPosition;
    private bool Spawned = false;
    private bool isHolding;
    private bool canPlace;
    private bool isHolderUsed;

    void Start()
    {
        placePointPos = this.transform.parent.position;
    }
    
    void Update()
    {
        placePointPos = this.transform.parent.position;
        buildingPos = this.transform.position;
        if (isHolding)
        {
            placePointPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            placePointPos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        }

        this.transform.parent.position = placePointPos;

        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && Spawned == false)
        {
            Instantiate(Ennemy, buildingPos, Quaternion.identity);
            Spawned = true;
        }

        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            Spawned = false;
        }
 
        if (holdersCount == neededHolders && !isHolderUsed)
        {
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }
    }

    private void OnMouseDown()
    {
        if (isHolding && canPlace)
        {
            int layerMask = ~(LayerMask.GetMask("Buildings"));
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 100f, layerMask);
            if (rayHit.collider != null)
            {
                holderPosition = rayHit.collider.transform.position;
            }
            this.transform.parent.position = holderPosition;
            isHolding = false;
        }
        else
        {
            isHolding = true;
        }
        GameManager.Instance.GetIsHolding(isHolding);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Holders holder = collision.gameObject.GetComponent<Holders>();
        if (holder != null && this.isHolding)
        {
            holdersCount++;
            isHolderUsed = holder.isUsed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Holders holder = collision.gameObject.GetComponent<Holders>();
        if (holder != null && this.isHolding)
        {
            if (canPlace)
            {
                holder.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                holder.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Holders holder = collision.gameObject.GetComponent<Holders>();
        if (holder != null)
        {
            holder.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            holdersCount--;
        }
    }
}
