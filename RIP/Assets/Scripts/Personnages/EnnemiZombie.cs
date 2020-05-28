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
        hp = ennemiValues.zombieHp;//*Nmanches/valeur
        speed = ennemiValues.zombieSpd;
        attack = ennemiValues.zombieAtk;
        level = ennemiValues.level;
        dropXP = ennemiValues.dropXP;
        rigid2d = this.GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<PolygonCollider2D>();
        invincibleTime = ennemiValues.invincibleTime;
        preparingAttack = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Attack();

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
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if (GameManager.Instance.PlayerPosition.x - this.rigid2d.position.x > -1 && GameManager.Instance.PlayerPosition.x - this.rigid2d.position.x < 1 &&
            GameManager.Instance.PlayerPosition.y - this.rigid2d.position.y > -1 && GameManager.Instance.PlayerPosition.y - this.rigid2d.position.y < 1 && preparingAttack == false)
        {
            preparingAttack = true;
        }

        if (preparingAttack == true)
        {
            dashAttackTimer += Time.deltaTime;
            if (dashAttackTimer > 3)
            {
                preparingAttack = false;
                dashAttackTimer = 0;
            }
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
            if (!invincible)
            {
                Damages();
            }
        }
    }
}
