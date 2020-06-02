using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;
    [SerializeField] private int knockbackStrenght;
    [SerializeField] private float knockbackDuration;
    
    private Vector2 originalPosition;
    private Vector2 knockbackDirection;

    private PolygonCollider2D thisCollider;

    private int damages;

    private void Awake()
    {
        thisCollider = this.GetComponent<PolygonCollider2D>();
        thisCollider.enabled = false;
    }
    
    void Update()
    {
        damages = playerValues.shovelDamages;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.gameObject.name + " touche " + collision + " et lui inflige " + damages);
        GameManager.Instance.DamageEnnemi(damages);
        if (collision.CompareTag("Ennemie"))
        {
            
            Rigidbody2D ennemieBody = collision.GetComponent<Rigidbody2D>();
            if (ennemieBody != null)
            {
                ennemieBody.isKinematic = false;
                ennemieBody.AddForce(knockbackDirection * knockbackStrenght, ForceMode2D.Impulse);
                StartCoroutine(KnockBack(ennemieBody));
            }
        }
    }

    private IEnumerator KnockBack(Rigidbody2D ennemie)
    {
        yield return new WaitForSeconds(knockbackDuration);
        if (ennemie != null)
        {
            ennemie.velocity = Vector2.zero;
            ennemie.isKinematic = true;
        }
    }
}
