using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiZombie : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    private BoxCollider2D box2d;
    //private SpriteRenderer spriteRend;

    private float dashAttackTimer = 0;
    private bool turnFlag = false;

    void Start()
    {
        hp = ennemiValues.zombieHp;
        speed = ennemiValues.zombieSpd;
        attack = ennemiValues.zombieAtk;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        //spriteRend = this.GetComponent<SpriteRenderer>();
        preparingAttack = false;
    }

    void Update()
    {
        Attack();

        if(preparingAttack == true)
        {
            dashAttackTimer += Time.deltaTime;
            if(dashAttackTimer > 3)
            {
                Debug.Log("je touche !");
                preparingAttack = false;
                this.box2d.enabled = false;
                dashAttackTimer = 0;
            }
        }

        if(joueurNull == false)
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

        if(this.hp <= 0)
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
        //Instantiate(flesh, this.transform.position)
        //if(animTimer >= temps)
        //Destroy(this.gameObject)
    }

    void Attack()
    {
        if (Joueur.x - this.rigid2d.position.x > -1.5 && Joueur.x - this.rigid2d.position.x < 1.5 &&
            Joueur.y - this.rigid2d.position.y > -1.5 && Joueur.y - this.rigid2d.position.y < 1.5 && joueurNull == false && preparingAttack == false)
        {
            preparingAttack = true;
            Debug.Log("à la fin de l'envoi...");
            this.box2d.enabled = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if(player != null)
        {
            GameManager.Instance.DamagePlayer(this.attack);
        }
    }
}
