using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSlime : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject slime;
    

    private float dashAttackTimer = 0;

    void Start()
    {
        base.fleePoint = this.transform.position;
        base.damageText = GameManager.Instance.HealthChangeText;
        base.damageTextTime = GameManager.Instance.GetAnimationTimes(damageText.GetComponentInChildren<Animator>(), "Health change");
        hp = ennemiValues.slimeHp;//*Nmanches/valeur
        speed = ennemiValues.slimeSpd;
        attack = ennemiValues.slimeAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        if (GameManager.Instance.DayCount > this.level)
        {
            base.LevelUp();
            ennemiValues.level = this.level;
            ennemiValues.slimeHp = this.hp;
            ennemiValues.slimeSpd = this.speed;
            ennemiValues.slimeAtk = this.attack;
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
        if (!flee)
        {
            Attack();
        }

        if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GameManager.Instance.PlayerPosition.x < this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
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

    void Attack()
    {
        dashAttackTimer += Time.deltaTime;
        if (dashAttackTimer >= 1.5f && dashAttackTimer <= 2)
        {
            this.speed = ennemiValues.slimeSpd * 2;
        }
        else if (dashAttackTimer >= 2)
        {
            this.speed = ennemiValues.slimeSpd;
            dashAttackTimer = 0;
            preparingAttack = false;
        }
        else
        {
            this.speed = 0;
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void MortEnnemi()
    {
        Instantiate(slime, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        AudioManager.Instance.Play("DeathSlime");
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
    }
}
