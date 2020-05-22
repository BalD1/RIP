﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiZombie : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject flesh;

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    void Start()
    {
        hp = ennemiValues.zombieHp;//*Nmanches/valeur
        speed = ennemiValues.zombieSpd;
        attack = ennemiValues.zombieAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        preparingAttack = false;
    }

    void Update()
    {
        Attack();

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
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if (GameManager.Instance.PlayerPosition.x - this.rigid2d.position.x > -1.4 && GameManager.Instance.PlayerPosition.x - this.rigid2d.position.x < 1.4 &&
            GameManager.Instance.PlayerPosition.y - this.rigid2d.position.y > -1.4 && GameManager.Instance.PlayerPosition.y - this.rigid2d.position.y < 1.4 && preparingAttack == false)
        {
            preparingAttack = true;
            this.box2d.enabled = true;
        }


        if (preparingAttack == true)
        {
            dashAttackTimer += Time.deltaTime;
            if (dashAttackTimer > 3)
            {
                preparingAttack = false;
                this.box2d.enabled = false;
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
            this.hp -= GameManager.Instance.SendDamagesEnnemi();
            GameManager.Instance.DamageEnnemi(0);
        }
    }
}
