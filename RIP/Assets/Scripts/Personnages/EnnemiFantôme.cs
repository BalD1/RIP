using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFantôme : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject ectoplasm;

    private float invincibilityTimer = 0;

    void Start()
    {
        hp = ennemiValues.fantômeHp;//*Nmanches/valeur
        speed = ennemiValues.fantômeSpd;
        attack = ennemiValues.fantômeAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        rigid2d = this.GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<PolygonCollider2D>();
        invincibleTime = ennemiValues.invincibleTime;
        preparingAttack = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GameManager.Instance.PlayerPosition.x < this.transform.position.x)
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
        if (invincible)
        {
            InvincibleClipping();
        }
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            dayFlag = false;
        }
        else if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && !dayFlag)
        {
            LevelUp();
            dayFlag = true;
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
        GameManager.Instance.ExperienceToPlayer = dropXP;
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
            if (!invincible)
            {
                Damages();
            }
        }
    }
}
