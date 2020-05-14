using System.Collections;
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
        if (preparingAttack == false)
        {
            if (Random.Range(0, 800) == 0)
            {
                preparingAttack = true;
                this.speed = 0;
            }

            if (dashAttackTimer != 0)
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
            if (dashAttackTimer >= 2 && dashAttackTimer <= 4)
            {
                this.speed = 3f;
                preparingAttack = false;
            }
            if (dashAttackTimer >= 4)
            {
                this.speed = 1.5f;
                dashAttackTimer = 0;
            }
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
        Destroy(this.gameObject);
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
