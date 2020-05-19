using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFantôme : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject ectoplasm;

    private BoxCollider2D box2d;

    private float invincibilityTimer = 0;

    void Start()
    {
        hp = ennemiValues.fantômeHp;//*Nmanches/valeur
        speed = ennemiValues.fantômeSpd;
        attack = ennemiValues.fantômeAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        preparingAttack = false;
    }

    void Update()
    {
        if (Player.Joueur.position.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Player.Joueur.position.x < this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        invincibilityTimer += Time.deltaTime;
        if(invincibilityTimer >= 2)
        {
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            if(invincibilityTimer >= 4)
            {
                this.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                invincibilityTimer = 0;
            }
        }

        if (this.hp <= 0)
        {
            MortEnnemi();
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void MortEnnemi()
    {
        Instantiate(ectoplasm, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        AudioManager.Instance.Play("DeathGhost");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GameManager.Instance.DamagePlayer(this.attack);
        }

        Shovel shovel = collision.gameObject.GetComponent<Shovel>();
        FireBall fireball = collision.gameObject.GetComponent<FireBall>();

        if (shovel != null || fireball != null)
        {
            this.hp -= GameManager.Instance.SendDamagesEnnemi();
            GameManager.Instance.DamageEnnemi(0);
        }
    }
}
