using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiZombie : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject flesh;

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;
    private bool turnFlag = false;

    void Start()
    {
        hp = ennemiValues.zombieHp;//*Nmanches/valeur
        speed = ennemiValues.zombieSpd;
        attack = ennemiValues.zombieAtk;
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

        if (Joueur.position.x > this.transform.position.x && turnFlag == false)
        {
            this.transform.Rotate(0, 180, 0);
            turnFlag = true;
        }
        else if (Joueur.position.x < this.transform.position.x && turnFlag == true)
        {
            this.transform.Rotate(0, 180, 0);
            turnFlag = false;
        }

        if(this.hp <= 0)
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
        Instantiate(flesh, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void Attack()
    {
        if (Joueur.position.x - this.rigid2d.position.x > -1.5 && Joueur.position.x - this.rigid2d.position.x < 1.5 &&
            Joueur.position.y - this.rigid2d.position.y > -1.5 && Joueur.position.y - this.rigid2d.position.y < 1.5 && preparingAttack == false)
        {
            preparingAttack = true;
            Debug.Log("à la fin de l'envoi...");
            this.box2d.enabled = true;
        }
    }

}
