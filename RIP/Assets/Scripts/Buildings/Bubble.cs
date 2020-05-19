using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField] GameObject building;
    [SerializeField] Animator bubblesAnimator;

    private void Awake()
    {
        bubblesAnimator = this.gameObject.GetComponent<Animator>();
    }

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
        if (this.gameObject != null && this.isActiveAndEnabled)
        {
            if (this.name == "DestoyButton")
            {
                Destroy(UIManager.Instance.SendActiveBuilding());
                UIManager.Instance.GetDestroyBubbleState(false);
                UIManager.Instance.GetActiveBuilding(null);
            }
            else if (this.name == "UnlockButton")
            {
                if (UIManager.Instance.SendEnoughRessources())
                {
                    Debug.Log("yo");
                    UIManager.Instance.GetCanUnlock(true);
                }
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
                    }
                }
            }
        }
    }

}
