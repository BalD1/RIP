using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : MonoBehaviour
{
    [SerializeField] private List<Sprite> flowerSprites;

    [SerializeField] private GameObject flowersDrop;

    [SerializeField] private int spawnChances;

    private SpriteRenderer spriteRenderer;

    private bool gathered;
    private bool dayFlag;
    private bool changeSpriteFlag;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        dayFlag = false;
        gathered = false;
        changeSpriteFlag = false;
    }


    private void Update()
    {
        if (!dayFlag && gathered && GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            if (Random.Range(0, spawnChances) == 0)
            {
                this.gathered = false;
                this.spriteRenderer.enabled = true;
            }
            dayFlag = true;
        }
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            dayFlag = false;
            changeSpriteFlag = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && !gathered)
        {
            GameManager.Instance.PlayerCanInteract = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && !gathered)
        {
            if (GameManager.Instance.PlayerInteracted)
            {
                gathered = true;
                this.spriteRenderer.enabled = false;
                this.spriteRenderer.sprite = flowerSprites[Random.Range(0, flowerSprites.Count)];
                Instantiate(flowersDrop, this.transform.position, Quaternion.identity);
                GameManager.Instance.PlayerInteracted = false;
                GameManager.Instance.PlayerCanInteract = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GameManager.Instance.PlayerCanInteract = false;
        }
    }
}
