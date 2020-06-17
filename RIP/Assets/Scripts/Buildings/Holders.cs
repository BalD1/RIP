using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holders : MonoBehaviour                // Florian
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

    private Animator bubblesAnimator;

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
    }

    private void Update()
    {
        if (UIManager.Instance.SendActiveHolder() == null)
        {
            ActivateGhostBuilding(null);
        }
        if (this.isActive)
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
        if (UIManager.Instance.BuildDisplayActive == false)
        {
            if (this.isActive)
            {
                UIManager.Instance.GetBuildBubblesState(false);
                UIManager.Instance.BuildDisplayActive = false;
                this.isActive = false;
                this.spriteRenderer.color = originalColor;
                UIManager.Instance.GetActiveHolder(null);
                if (UIManager.Instance.WasHUDHidden)
                {
                    UIManager.Instance.HUDAnimatorBool = false;
                }
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

    private void OnMouseEnter()
    {
        GameManager.Instance.MouseIsOverSomething = true;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && GameManager.Instance.SendGameState() == GameManager.GameState.InGame)
        {
            bubblesAnimator = UIManager.Instance.BubblesAnimator;
            if (!this.isUsed && UIManager.Instance.SendActiveBuilding() == null)
            {
                if (UIManager.Instance.SendBuildBubblesState() == false)
                {
                    GameManager.Instance.GetBubblesHolderPosition(bubblesPosition);
                    UIManager.Instance.BuildDisplayActive = true;
                    UIManager.Instance.GetBuildBubblesState(true);
                    UIManager.Instance.GetActiveHolder(this.gameObject);
                    this.isActive = true;
                    this.spriteRenderer.color = Color.gray;
                    if (UIManager.Instance.HUDAnimatorBool == false)
                    {
                        UIManager.Instance.HUDAnimatorBool = true;
                        UIManager.Instance.WasHUDHidden = true;
                    }
                    else
                    {
                        UIManager.Instance.WasHUDHidden = false;
                    }
                }
                else if (UIManager.Instance.SendActiveHolder() == this.gameObject)
                {
                    UIManager.Instance.GetBuildBubblesState(false);
                    UIManager.Instance.BuildDisplayActive = false;
                    this.isActive = false;
                    this.spriteRenderer.color = originalColor;
                    UIManager.Instance.GetActiveHolder(null);
                    if (UIManager.Instance.WasHUDHidden)
                    {
                        UIManager.Instance.HUDAnimatorBool = false;
                    }
                }
                else if (UIManager.Instance.SendActiveHolder() != this.gameObject)
                {
                    UIManager.Instance.BuildDisplayActive = false;
                    Invoke("OnMouseDown", 0f);
                }
            }
            else if (!this.isUsed && UIManager.Instance.SendActiveBuilding() != null)
            {
                UIManager.Instance.BuildDisplayActive = false;
                Invoke("OnMouseDown", 0f);
            }
        }
    }

    private void OnMouseExit()
    {
        GameManager.Instance.MouseIsOverSomething = false;
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
            if (UIManager.Instance.WasHUDHidden)
            {
                UIManager.Instance.HUDAnimatorBool = false;
            }
        }
    }





}
