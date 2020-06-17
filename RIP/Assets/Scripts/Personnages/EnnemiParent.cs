using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnnemiParent : MonoBehaviour//QuentinLamy
{
    protected Animator animator;

    protected GameObject damageText;

    protected float speed;
    protected float invincibleTime;
    protected float damageTextTime;

    protected int hp;
    protected int attack;
    protected int level;
    protected int dropXP;

    protected Rigidbody2D rigid2d;

    protected Collider2D hitbox;

    protected bool preparingAttack;
    protected bool invincible;
    protected bool dayFlag;
    protected bool flee;
    protected bool spawnFlag = false;

    protected SpriteRenderer spriteRenderer;

    protected Vector2 jPos;
    private Vector2 targetDirection;
    protected Vector3 fleePoint;

    void Start()
    {
        rigid2d = this.GetComponent<Rigidbody2D>();
    }

    protected void Movement()
    {
        if (!flee)
        {
            targetDirection = GameManager.Instance.PlayerPosition;
        }

        if (preparingAttack == false)
        {
            rigid2d.position = Vector2.MoveTowards(rigid2d.position, targetDirection, speed * Time.deltaTime);
        }
        if (flee && this.transform.position == fleePoint)
        {
            Destroy(this.gameObject);
        }
    }

    protected void LevelUp()
    {
        level++;
        hp = (int)( ((hp / 2) + (level /2)) * 1.69 );
        attack = (int)((attack + level) / 2.1);
        dropXP = (int)(5 * (level - 1) + (dropXP / 1.1f));
    }

    protected void Damages()
    {
        hp -= GameManager.Instance.SendDamagesEnnemi();
        Invincible();
        GameObject damage = Instantiate(damageText, this.transform);
        damage.GetComponentInChildren<TextMeshProUGUI>().text = "- " + GameManager.Instance.SendDamagesEnnemi().ToString();
        StartCoroutine(DamageText((damageTextTime), damage));
    }

    IEnumerator DamageText(float time, GameObject text)
    {
        yield return new WaitForSeconds(time);

        Destroy(text);
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

    protected void FleeAtDay()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            flee = true;
            targetDirection = fleePoint;
            speed = 6;
        }
    }



}
