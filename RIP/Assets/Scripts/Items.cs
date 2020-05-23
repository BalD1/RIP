using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{

    private bool move;
    private Vector2 onPickupDirection;
    

    private void Update()
    {
        switch (this.name)
        {
            case string flesh when flesh.Contains("Flesh"):
                onPickupDirection = Camera.main.ScreenToWorldPoint(UIManager.Instance.FleshDisplay.rectTransform.transform.position);
                break;
            case string bone when bone.Contains("Bone"):
                onPickupDirection = Camera.main.ScreenToWorldPoint(UIManager.Instance.BoneDisplay.rectTransform.transform.position);
                break;
            case string slime when slime.Contains("Slime"):
                onPickupDirection = Camera.main.ScreenToWorldPoint(UIManager.Instance.SlimeDisplay.rectTransform.transform.position);
                break;
            case string ectoplasm when ectoplasm.Contains("Ectoplasm"):
                onPickupDirection = Camera.main.ScreenToWorldPoint(UIManager.Instance.EctoplasmDisplay.rectTransform.transform.position);
                break;
            case string flower when flower.Contains("flower"):
                onPickupDirection = Camera.main.ScreenToWorldPoint(UIManager.Instance.FleshDisplay.rectTransform.transform.position);
                break;
            default:
                Debug.Log(this.name + " not recognized in Item script");
                break;
        }
        if (move)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, onPickupDirection, 0.2f);
        }

        if (this.transform.position.x == onPickupDirection.x && this.transform.position.y == onPickupDirection.y && move)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            move = true;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
