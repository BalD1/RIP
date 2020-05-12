using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFantôme : EnnemiParent
{
    [SerializeField]
    private EnnemiValues ennemiValues;

    [SerializeField]
    private GameObject ectoplasm;

    private BoxCollider2D box2d;

    private float dashAttackTimer = 0;

    void Start()
    {
        hp = ennemiValues.fantômeHp;//*Nmanches/valeur
        speed = ennemiValues.fantômeSpd;
        attack = ennemiValues.fantômeAtk;
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

    void FixedUpdate()
    {
        Movement();
    }

    private void MortEnnemi()
    {
        Instantiate(ectoplasm, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        Destroy(this.gameObject);
    }
}
