using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSquelette : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject bone;

    [SerializeField]
    private GameObject projectile;

    private float dashAttackTimer = 0;

    void Start()
    {
        base.fleePoint = this.transform.position;
        base.damageText = GameManager.Instance.HealthChangeText;
        base.damageTextTime = GameManager.Instance.GetAnimationTimes(damageText.GetComponentInChildren<Animator>(), "Health change");
        hp = ennemiValues.squeletteHp;//*Nmanches/valeur
        speed = ennemiValues.squeletteSpd;
        attack = ennemiValues.squeletteAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        if (GameManager.Instance.DayCount > this.level)
        {
            base.LevelUp();
            ennemiValues.level = this.level;
            ennemiValues.squeletteHp = this.hp;
            ennemiValues.squeletteSpd = this.speed;
            ennemiValues.squeletteAtk = this.attack;
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

        if (this.hp <= 0)
        {
            MortEnnemi();
        }
        if (invincible)
        {
            InvincibleClipping();
        }

        if (!flee)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if((GameManager.Instance.PlayerPosition.x < (this.transform.position.x + 2f)) && (GameManager.Instance.PlayerPosition.y < (this.transform.position.y + 2f)) && (GameManager.Instance.PlayerPosition.y > (this.transform.position.y - 2f))
           || (GameManager.Instance.PlayerPosition.x > (this.transform.position.x - 2f)) && (GameManager.Instance.PlayerPosition.y < (this.transform.position.y + 2f)) && (GameManager.Instance.PlayerPosition.y > (this.transform.position.y - 2f))
           || (GameManager.Instance.PlayerPosition.y < (this.transform.position.y + 2f)) && (GameManager.Instance.PlayerPosition.x < (this.transform.position.x + 2f)) && (GameManager.Instance.PlayerPosition.x > (this.transform.position.x - 2f))
           || (GameManager.Instance.PlayerPosition.y > (this.transform.position.y - 2f)) && (GameManager.Instance.PlayerPosition.x < (this.transform.position.x + 2f)) && (GameManager.Instance.PlayerPosition.x > (this.transform.position.x - 2f)))
        {
            if (flee)
            {
                Movement();
            }
        }
        else
        {
            Movement();
        }
    }

    private void MortEnnemi()
    {
        Instantiate(bone, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        AudioManager.Instance.Play("DeathSkeleton");
        GameManager.Instance.ExperienceToPlayer = dropXP;
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if (GameManager.Instance.PlayerPosition.y > (this.transform.position.y - 0.5f)
            && GameManager.Instance.PlayerPosition.y < (this.transform.position.y + 0.5f))
        {
            jPos.y = 0;
        }
        else if (GameManager.Instance.PlayerPosition.y > this.transform.position.y)
        {
            jPos.y = 1;
        }
        else
        {
            jPos.y = -1;
        }

        if (GameManager.Instance.PlayerPosition.x > (this.transform.position.x - 0.5f)
            && GameManager.Instance.PlayerPosition.x < (this.transform.position.x + 0.5f))
        {
            jPos.x = 0;
        }
        else if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
        {
            jPos.x = 1;
        }
        else
        {
            jPos.x = -1;
        }

        if (Random.Range(0, 600) == 0)
        {
            GameObject shot = Instantiate(projectile, this.transform.position, Quaternion.identity);
            shot.gameObject.GetComponent<Rigidbody2D>().AddForce(jPos*300);
            jPos.x = 0;
            jPos.y = 0;
        }
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
