using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSquelette : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    private BoxCollider2D box2d;
    //private SpriteRenderer spriteRend;

    private float dashAttackTimer = 0;
    private bool turnFlag = false;

    void Start()
    {
        hp = ennemiValues.squeletteHp;
        speed = ennemiValues.squeletteSpd;
        attack = ennemiValues.squeletteAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        //spriteRend = this.GetComponent<SpriteRenderer>();
        preparingAttack = false;
    }

    void Update()
    {
        Attack();

        if (joueurNull == false)
        {
            if (Joueur.x > this.transform.position.x && turnFlag == false)
            {
                this.transform.Rotate(0, 180, 0);
                turnFlag = true;
            }
            else if (Joueur.x < this.transform.position.x && turnFlag == true)
            {
                this.transform.Rotate(0, 180, 0);
                turnFlag = false;
            }
        }

        if (this.hp <= 0)
        {
            //animTimer++
            MortEnnemi();
        }

    }

    void FixedUpdate()
    {
        Movement();
    }

    private void MortEnnemi()
    {
        //"animation" mort
        //Instantiate(bone, this.transform.position)
        //if(animTimer >= temps)
        //Destroy(this.gameObject)
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GameManager.Instance.DamagePlayer(this.attack);
        }
    }
}
