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

    private Animator shovelAnimator;

    private float shovelAnimTimer;
    private float shovelAnimTime;

    private bool animFlag;

    private int damages;

    private void Awake()
    {
        thisCollider = this.GetComponent<PolygonCollider2D>();
        thisCollider.enabled = false;
        shovelAnimator = this.GetComponent<Animator>();
        shovelAnimTime = 0.75f;         // valeur de test en attendant les graphs
        shovelAnimTimer = shovelAnimTime;
    }

    void Start()
    {

    }
    
    void Update()
    {
        damages = playerValues.shovelDamages;
        
        if (thisCollider.isActiveAndEnabled)
        {
            if (!animFlag)
            {
                shovelAnimator.Play("shovelAnim");
                animFlag = true;
            }
            shovelAnimTimer = Mathf.Clamp(shovelAnimTimer - Time.deltaTime, 0, shovelAnimTime);
            if (shovelAnimTimer == 0)
            {
                thisCollider.enabled = false;
                shovelAnimTimer = shovelAnimTime;
                animFlag = false;
            }
        }
    }

    public void Activate(string lookDirection)
    {
        thisCollider.enabled = true;
        switch(lookDirection)
        {
            case "right":
                shovelAnimator.SetTrigger("lookright");
                knockbackDirection = Vector2.right;
                break;

            case "left":
                shovelAnimator.SetTrigger("lookleft");
                knockbackDirection = Vector2.left;
                break;

            case "front":
                shovelAnimator.SetTrigger("lookfront");
                knockbackDirection = Vector2.up;
                break;

            case "back":
                shovelAnimator.SetTrigger("lookback");
                knockbackDirection = Vector2.down;
                break;

            default:
                Debug.Log(lookDirection + " not recognized as a direction");
                break;
                
        }
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
