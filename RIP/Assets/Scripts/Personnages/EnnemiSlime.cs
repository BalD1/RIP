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
    private bool turnFlag = false;

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
        Instantiate(slime, this.transform.position, Quaternion.identity);
        //timer
        //animator.SetBool(Mort, true);
        Destroy(this.gameObject);
    }
}
