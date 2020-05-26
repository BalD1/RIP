using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bars : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Text text;
    [SerializeField] private PlayerValues playerValues;

    private void Awake()
    {
        this.text.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEvent)
    {
        this.text.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEvent)
    {
        this.text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.name.Equals("HPBar"))
        {
            this.text.text = playerValues.HpValue.ToString() + " / " + playerValues.maxHP.ToString();
        }
        else
        {
            this.text.text = playerValues.xpAmount.ToString() + " / " + playerValues.xpNeeded.ToString();
        }
    }

}
