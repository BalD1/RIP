using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holders : MonoBehaviour
{
    [SerializeField] private GameObject bubblesHolder;

    [SerializeField] private GameObject tomb;
    [SerializeField] private GameObject coffin;
    [SerializeField] private GameObject sewer;
    [SerializeField] private GameObject circle;

    [SerializeField] private GameObject ghostTomb;
    [SerializeField] private GameObject ghostCoffin;
    [SerializeField] private GameObject ghostSewer;
    [SerializeField] private GameObject ghostCircle;

    [SerializeField] private bool isLocked;
    [SerializeField] private int unlockFleshCost;
    [SerializeField] private int unlockBoneCost;
    [SerializeField] private int unlockSlimeCost;
    [SerializeField] private int unlockEctoplasmCost;


    List<GameObject> ghostBuildings = new List<GameObject>();

    private Vector2 bubblesPosition;

    private Color originalColor;

    private bool isUsed;
    private bool isActive;

    private string selectedBuilding;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        originalColor = this.spriteRenderer.color;
        ghostBuildings.Add(ghostTomb);
        ghostBuildings.Add(ghostCoffin);
        ghostBuildings.Add(ghostSewer);
        ghostBuildings.Add(ghostCircle);
        bubblesPosition = bubblesHolder.transform.position;
    }

    void Start()
    {
        isUsed = false;
        isActive = false;
        if (isLocked)
        {
            this.LockDown();
        }
    }

    private void LockDown()
    {
        this.spriteRenderer.color = Color.black;
        this.isUsed = true;
    }

    private void Unlock()
    {
        this.spriteRenderer.color = originalColor;
        this.isLocked = false;
        this.isUsed = false;
    }

    private void Update()
    {
        if (UIManager.Instance.SendActiveHolder() == null)
        {
            ActivateGhostBuilding(null);
        }
        if (this.isActive && !this.isLocked)
        {
            if (UIManager.Instance.SendActiveHolder() != this.gameObject)
            {
                this.isActive = false;
            }
        }
        selectedBuilding = UIManager.Instance.SendSelectedBuildingName();

        if (!this.isUsed && this.isActive)
        {
            switch(selectedBuilding)
            {
                case "tomb":
                    ActivateGhostBuilding(ghostTomb);
                    break;
                case "sewer":
                    ActivateGhostBuilding(ghostSewer);
                    break;
                case "coffin":
                    ActivateGhostBuilding(ghostCoffin);
                    break;
                case "circle":
                    ActivateGhostBuilding(ghostCircle);
                    break;
                default:
                    ActivateGhostBuilding(null);
                    break;
            }
        }
        Debug.Log(this.isLocked + " " + this.isActive);
        if (this.isLocked && this.isActive)
        {
            if (UIManager.Instance.SendCanUnlock())
            {
                this.Unlock();
                UIManager.Instance.GetCanUnlock(false);
                UIManager.Instance.GetUnlockBubbleState(false);
            }
        }
    }

    private void ActivateGhostBuilding(GameObject building)
    {
        foreach(GameObject ghostBuilding in ghostBuildings)
        {
            if (building == ghostBuilding)
            {
                ghostBuilding.SetActive(true);
                ghostBuilding.transform.position = this.transform.position;
            }
            else
            {
                ghostBuilding.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && GameManager.Instance.SendGameState() == GameManager.GameState.InGame)
        {
            if (this.isLocked)
            {
                if (UIManager.Instance.SendBuildBubblesState() == false && UIManager.Instance.SendDestroyBubbleState() == false && UIManager.Instance.SendUnlockBubbleState() == false)
                {
                    GameManager.Instance.GetBubblesHolderPosition(bubblesPosition);
                    UIManager.Instance.GetUnlockBubbleState(true);
                    UIManager.Instance.GetBuildingsCosts(unlockFleshCost, unlockBoneCost, unlockSlimeCost, unlockEctoplasmCost);
                    this.isActive = true;
                }
                else if (UIManager.Instance.SendUnlockBubbleState() == true)
                {
                    GameManager.Instance.GetBubblesHolderPosition(Vector2.zero);
                    UIManager.Instance.GetUnlockBubbleState(false);
                    this.isActive = false;
                }
            }
            if (!this.isUsed && UIManager.Instance.SendActiveBuilding() == null)
            {
                if (UIManager.Instance.SendBuildBubblesState() == false && UIManager.Instance.SendUnlockBubbleState() == false)
                {
                    GameManager.Instance.GetBubblesHolderPosition(bubblesPosition);
                    UIManager.Instance.GetBuildBubblesState(true);
                    UIManager.Instance.GetActiveHolder(this.gameObject);
                    this.isActive = true;
                    this.spriteRenderer.color = Color.gray;
                }
                else if (UIManager.Instance.SendActiveHolder() == this.gameObject)
                {
                    UIManager.Instance.GetBuildBubblesState(false);
                    this.isActive = false;
                    this.spriteRenderer.color = originalColor;
                    UIManager.Instance.GetActiveHolder(null);
                }
            }
        }
    }

    public bool IsUsed()
    {
        return this.isUsed;
    }

    public bool IsActive()
    {
        return this.isActive;
    }

    public Color OriginalColor()
    {
        return this.originalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tomb building = collision.GetComponent<Tomb>();
        if (building != null)
        {
            if (!building.name.Contains("Ghost"))
            {
                this.isUsed = true;
                this.spriteRenderer.color = originalColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tomb building = collision.GetComponent<Tomb>();
        if (building != null)
        {
            if (!building.name.Contains("Ghost"))
            {
                this.isUsed = false;
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (UIManager.Instance.SendActiveHolder() == this.gameObject && UIManager.Instance.SendBuildBubblesState())
        {
            UIManager.Instance.GetBuildBubblesState(false);
            UIManager.Instance.GetActiveHolder(null);
            this.isActive = false;
            this.spriteRenderer.color = originalColor;
        }
    }





}
