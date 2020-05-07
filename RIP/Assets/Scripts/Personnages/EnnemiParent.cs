using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiParent : MonoBehaviour
{
    [SerializeField]
    protected Transform Joueur;

    [SerializeField]
    protected int hp;

    [SerializeField]
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
            rigid2d.position = Vector2.MoveTowards(rigid2d.position, Joueur.position, speed * Time.deltaTime);
            this.rigid2d.MovePosition(rigid2d.position);
        }
    }



    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GameManager.Instance.DamagePlayer(this.attack);
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
