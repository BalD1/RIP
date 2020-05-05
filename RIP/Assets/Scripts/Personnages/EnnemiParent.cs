using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiParent : MonoBehaviour
{
    // Script parent des ennemis, pour mettre les scripts qui seront communs à tout les ennemis

    [SerializeField]
    protected int hp;

    [SerializeField]
    protected float speed;

    protected int attack;

    protected bool joueurNull = true;

    protected Vector2 Joueur;

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
        if(joueurNull == false && preparingAttack == false)
        {
            Debug.Log("je bouge");
            rigid2d.position = Vector2.MoveTowards(rigid2d.position, Joueur, speed * Time.deltaTime);
            this.rigid2d.MovePosition(rigid2d.position);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            joueurNull = false;
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            Joueur = player.gameObject.GetComponent<Transform>().position;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            joueurNull = true;
        }
    }
}
