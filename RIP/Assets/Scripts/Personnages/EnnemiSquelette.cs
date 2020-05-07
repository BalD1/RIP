﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSquelette : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject bone;

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    void Start()
    {
        hp = ennemiValues.squeletteHp;//*Nmanches/valeur
        speed = ennemiValues.squeletteSpd;
        attack = ennemiValues.squeletteAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        preparingAttack = false;
    }

    void Update()
    {
        Attack();

        if (Joueur.position.x > this.transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Joueur.position.x < this.transform.position.x)
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
        Instantiate(bone, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if(preparingAttack == false)
        {
            if(Random.Range(0, 800) == 0)
            {
                preparingAttack = true;
                this.speed = 0;
            }

            if(dashAttackTimer != 0)
            {
                dashAttackTimer += Time.deltaTime;
                if (dashAttackTimer >= 6)
                {
                    this.speed = 1.5f;
                    dashAttackTimer = 0;
                }
            }
        }
        else
        {
            dashAttackTimer += Time.deltaTime;
            if(dashAttackTimer >= 2 && dashAttackTimer <= 6)
            {
                this.speed = 2.5f;
                preparingAttack = false;
            }
            if(dashAttackTimer >= 6)
            {
                this.speed = 1.5f;
                dashAttackTimer = 0;
            }
        }
    }
}
