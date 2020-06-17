using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFantôme : EnnemiParent//QuentinLamy
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject ectoplasm;

    private float invincibilityTimer = 0;

    void Start()
    {
        base.fleePoint = this.transform.position;
        base.damageText = GameManager.Instance.HealthChangeText;
        base.damageTextTime = GameManager.Instance.GetAnimationTimes(damageText.GetComponentInChildren<Animator>(), "Health change");
        hp = ennemiValues.fantômeHp;//*Nmanches/valeur
        speed = ennemiValues.fantômeSpd;
        attack = ennemiValues.fantômeAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        if (GameManager.Instance.NightCount > this.level)
        {
            base.LevelUp();
            ennemiValues.level = this.level;
            ennemiValues.fantômeHp = this.hp;
            ennemiValues.fantômeSpd = this.speed;
            ennemiValues.fantômeAtk = this.attack;
            ennemiValues.dropXP = this.dropXP;
        }
        rigid2d = this.GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<PolygonCollider2D>();
        invincibleTime = ennemiValues.invincibleTime;
        preparingAttack = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        base.FleeAtDay();
        if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GameManager.Instance.PlayerPosition.x < this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        invincibilityTimer += Time.deltaTime;
        if(invincibilityTimer >= 2 && !flee)
        {
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            this.speed = ennemiValues.fantômeSpd * 1.2f;
            if(invincibilityTimer >= 4)
            {
                this.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                invincibilityTimer = 0;
                this.speed = ennemiValues.fantômeSpd;
            }
        }

        if (flee)
        {
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }

        if (this.hp <= 0)
        {
            MortEnnemi();
        }
        if (invincible)
        {
            InvincibleClipping();
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

        GameManager.PlayerStats playerStats = GameManager.Instance.currentStats;
        playerStats.totalKills++;
        playerStats.ghostsKills++;
        GameManager.Instance.currentStats = playerStats;

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
            if (!invincible && GameManager.Instance.SendDamagesEnnemi() > 0)
            {
                Damages();
            }
        }

        Tomb tomb = collision.gameObject.GetComponent<Tomb>();
        if (tomb != null && spawnFlag == false)
        {
            spawnFlag = true;
            if (tomb.GetBuildingLevel() > 1)
            {
                for (int i = 1; i < tomb.GetBuildingLevel(); i++)
                {
                    base.LevelUp();
                }
            }
            if (tomb.IsFlowered())
            {
                this.attack /= 2;
                this.hp /= 2;
                this.speed /= 1.5f;
            }
        }
    }
}
