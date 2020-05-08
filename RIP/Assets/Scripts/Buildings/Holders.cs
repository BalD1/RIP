using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Holders : MonoBehaviour
{

    [SerializeField] private GameObject tomb;
    [SerializeField] private GameObject coffin;
    [SerializeField] private GameObject sewer;
    [SerializeField] private GameObject circle;

    [SerializeField] private GameObject ghostTomb;
    [SerializeField] private GameObject ghostCoffin;
    [SerializeField] private GameObject ghostSewer;
    [SerializeField] private GameObject ghostCircle;

    List<GameObject> ghostBuildings = new List<GameObject>();


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
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            if (!this.isUsed && UIManager.Instance.SendActiveBuilding() == null)
            {
                if (UIManager.Instance.SendBuildBubblesState() == false)
                {
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
            else
            {
                Debug.Log("used");
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
