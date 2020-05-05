using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSlime : EnnemiParent
{
    private BoxCollider2D box2d;
    //private SpriteRenderer spriteRend;

    private float dashAttackTimer = 0;
    private bool turnFlag = false;

    void Start()
    {
        hp = 2;
        speed = 1.75f;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
        //spriteRend = this.GetComponent<SpriteRenderer>();
        preparingAttack = false;
    }

    void Update()
    {
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
        //Instantiate(slime, this.transform.position)
        //if(animTimer >= temps)
        //Destroy(this.gameObject)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GameManager.Instance.DamagePlayer(1);
        }
    }
}
