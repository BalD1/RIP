﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSlime : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject slime;

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    void Start()
    {
        hp = ennemiValues.slimeHp;//*Nmanches/valeur
        speed = ennemiValues.slimeSpd;
        attack = ennemiValues.slimeAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        preparingAttack = false;
    }

    void Update()
    {
        Attack();

        if (Player.Joueur.position.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Player.Joueur.position.x < this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (this.hp <= 0)
        {
            MortEnnemi();
        }

    }

    void Attack()
    {
        dashAttackTimer += Time.deltaTime;
        if (dashAttackTimer >= 1.5f && dashAttackTimer <= 2)
        {
            this.speed = 5f;
        }
        else if (dashAttackTimer >= 2)
        {
            this.speed = 0;
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
            this.hp -= GameManager.Instance.SendDamagesEnnemi();
            GameManager.Instance.DamageEnnemi(0);
        }
    }
}
