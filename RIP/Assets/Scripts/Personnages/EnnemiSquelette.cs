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

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    private Vector2 jPos;

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
        if(Player.Joueur.position.y > (this.transform.position.y - 0.5f)
            && Player.Joueur.position.y < (this.transform.position.y + 0.5f))
        {
            jPos.y = 0;
        }
        else if(Player.Joueur.position.y > this.transform.position.y)
        {
            jPos.y = 1;
        }
        else
        {
            jPos.y = -1;
        }
        
        if (Player.Joueur.position.x > (this.transform.position.x - 0.5f)
            && Player.Joueur.position.x < (this.transform.position.x + 0.5f))
        {
            jPos.x = 0;
        }
        else if (Player.Joueur.position.x > this.transform.position.x)
        {
            jPos.x = 1;
        }
        else
        {
            jPos.x = -1;
        }

        if (this.hp <= 0)
        {
            MortEnnemi();
        }

        Attack();
    }

    void FixedUpdate()
    {
        if((Player.Joueur.position.x < (this.transform.position.x + 2f)) && (Player.Joueur.position.y < (this.transform.position.y + 2f)) && (Player.Joueur.position.y > (this.transform.position.y - 2f))
           || (Player.Joueur.position.x > (this.transform.position.x - 2f)) && (Player.Joueur.position.y < (this.transform.position.y + 2f)) && (Player.Joueur.position.y > (this.transform.position.y - 2f))
           || (Player.Joueur.position.y < (this.transform.position.y + 2f)) && (Player.Joueur.position.x < (this.transform.position.x + 2f)) && (Player.Joueur.position.x > (this.transform.position.x - 2f))
           || (Player.Joueur.position.y > (this.transform.position.y - 2f)) && (Player.Joueur.position.x < (this.transform.position.x + 2f)) && (Player.Joueur.position.x > (this.transform.position.x - 2f)))
        {
            
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
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if(Random.Range(0, 600) == 0)
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
            this.hp -= GameManager.Instance.SendDamagesEnnemi();
            GameManager.Instance.DamageEnnemi(0);
        }
    }
}
