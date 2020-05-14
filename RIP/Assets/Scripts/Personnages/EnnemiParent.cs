﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiParent : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected int hp;

    protected float speed;

    protected int attack;

    protected Rigidbody2D rigid2d;

    protected bool preparingAttack;

    void Start()
    {
        rigid2d = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    protected void Movement()
    {
        if(preparingAttack == false)
        {
            Debug.Log("je bouge");
            rigid2d.position = Vector2.MoveTowards(rigid2d.position, Player.Joueur.position, speed * Time.deltaTime);
            this.rigid2d.MovePosition(rigid2d.position);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Shovel shovel = collision.gameObject.GetComponent<Shovel>();
        FireBall fireball = collision.gameObject.GetComponent<FireBall>();

        if (shovel != null || fireball != null)
        {
            this.hp -= GameManager.Instance.SendDamagesEnnemi();
            GameManager.Instance.DamageEnnemi(0);
        }
    }
}
