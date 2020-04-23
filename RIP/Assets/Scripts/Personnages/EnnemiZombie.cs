using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiZombie : EnnemiParent
{
    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    void Start()
    {
        hp = 2;
        speed = 2;
        rigid2d = this.GetComponent<Rigidbody2D>();
        box2d = this.GetComponent<BoxCollider2D>();
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
        //essai de tourner la hitbox d'attaque du zombie pour regarder le joeuur (ne fonctionne pas)
        //this.box2d.transform.Rotate(0, 0, Vector2.Angle(this.rigid2d.position, Joueur));
    }

    void FixedUpdate()
    {
        Movement();
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
}
