using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiZombie : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject flesh;

    private float dashAttackTimer = 0;

    void Start()
    {
        base.fleePoint = this.transform.position;
        base.damageText = GameManager.Instance.HealthChangeText;
        base.damageTextTime = GameManager.Instance.GetAnimationTimes(damageText.GetComponentInChildren<Animator>(), "Health change");
        hp = ennemiValues.zombieHp;//*Nmanches/valeur
        speed = ennemiValues.zombieSpd;
        attack = ennemiValues.zombieAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        if (GameManager.Instance.DayCount > this.level)
        {
            base.LevelUp();
            ennemiValues.level = this.level;
            ennemiValues.zombieHp = this.hp;
            ennemiValues.zombieSpd = this.speed;
            ennemiValues.zombieAtk = this.attack;
            ennemiValues.dropXP = this.dropXP;
        }
        rigid2d = this.GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<PolygonCollider2D>();
        invincibleTime = ennemiValues.invincibleTime;
        preparingAttack = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        base.FleeAtDay();

        if (this.hp <= 0)
        {
            MortEnnemi();
        }
        if (invincible)
        {
            InvincibleClipping();
        }


        Turn();

    }

    void FixedUpdate()
    {
        Movement();
    }

    private void MortEnnemi()
    {
        Instantiate(flesh, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        AudioManager.Instance.Play("DeathZombie");
        GameManager.Instance.ExperienceToPlayer = dropXP;
        GameManager.PlayerStats playerStats = GameManager.Instance.currentStats;
        playerStats.kills++;
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
    }
}
