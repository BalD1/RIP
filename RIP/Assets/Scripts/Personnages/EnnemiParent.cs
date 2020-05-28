using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiParent : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;


    protected float speed;
    protected float invincibleTime;

    protected int hp;
    protected int attack;
    protected int level;
    protected int dropXP;

    protected Rigidbody2D rigid2d;

    protected Collider2D hitbox;

    protected bool preparingAttack;
    protected bool invincible;
    protected bool dayFlag;

    protected SpriteRenderer spriteRenderer;

    protected Vector2 jPos;

    void Start()
    {
        rigid2d = this.GetComponent<Rigidbody2D>();
    }

    protected void Movement()
    {
        if(preparingAttack == false)
        {
            rigid2d.position = Vector2.MoveTowards(rigid2d.position, GameManager.Instance.PlayerPosition, speed * Time.deltaTime);
            this.rigid2d.MovePosition(rigid2d.position);
        }
    }

    protected void LevelUp()
    {
        level++;
        hp = (int)( ((hp / 2) + (level /2)) * 1.4 );
        attack = (int)((attack + level) / 2.5);
        dropXP = (int)(5 * (level - 1) + (dropXP / 1.3f));
    }

    protected void Damages()
    {
        hp -= GameManager.Instance.SendDamagesEnnemi();
        Invincible();
    }

    private void Invincible()
    {
        invincible = true;
        StartCoroutine(Invincible(invincibleTime));
    }

    IEnumerator Invincible(float hurtTime)
    {
        hitbox.enabled = false;

        yield return new WaitForSeconds(hurtTime);

        CancelInvoke();
        spriteRenderer.enabled = true;
        invincible = false;
        hitbox.enabled = true;
        GameManager.Instance.DamageEnnemi(0);

    }

    protected void InvincibleClipping()
    {
        if (spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    protected void Turn()
    {
        if (GameManager.Instance.PlayerPosition.y > (this.transform.position.y - 2f)
            && GameManager.Instance.PlayerPosition.y < (this.transform.position.y + 2f))
        {
            jPos.y = 0;
            if (GameManager.Instance.PlayerPosition.x > this.transform.position.x)
            {
                jPos.x = 1;
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", true);
            }
            else
            {
                jPos.x = -1;
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Left", true);
                animator.SetBool("Right", false);
            }
        }
        else if (GameManager.Instance.PlayerPosition.y > this.transform.position.y)
        {
            jPos.y = 1;
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else
        {
            jPos.y = -1;
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
    }



}
