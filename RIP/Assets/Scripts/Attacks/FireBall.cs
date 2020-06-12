using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private float maxDistance;

    private int damages;

    private Animator animator;

    private float explosionTime;

    private Vector2 originalPosition;

    private Rigidbody2D thisBody;

    private SpriteRenderer spriteRenderer;

    private float playerLookAngle;

    private bool dSound = true;
    private void Awake()
    {
        AudioManager.Instance.Play("CreatingFire");
    }

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        thisBody = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        explosionTime = GameManager.Instance.GetAnimationTimes(animator, "Explosion");
        playerLookAngle = GameManager.Instance.PlayerLookAngle;
        Vector3 rotationAngle = Vector3.zero;
        rotationAngle.z = playerLookAngle;
        this.transform.eulerAngles = rotationAngle;
        damages = playerValues.fireBallDamages;
        originalPosition = this.transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(originalPosition, this.transform.position) > maxDistance)       // Détruit le spell si assez loin
        {
            DestroyThis();
        }

    }

    private void DestroyThis()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        thisBody.velocity = Vector2.zero;
        thisBody.isKinematic = true;
        animator.SetTrigger("Explosion");
        StartCoroutine(WaitForAnimationEnd());
    }

    IEnumerator WaitForAnimationEnd()
    {
        if(dSound)
        {
            AudioManager.Instance.Play("FireImpact");
            dSound = false;
        }
        yield return new WaitForSeconds(explosionTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ennemie")
        {
            GameManager.Instance.DamageEnnemi(damages);
            DestroyThis();
        }
        
    }
}
