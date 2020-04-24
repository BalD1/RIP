using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    [SerializeField]
    private GameObject fireBall;

    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite backSprite;
    [SerializeField]
    private Sprite sideSprite;

    private int HP;
    private int shovelDamages;
    private int fireBallDamages;
    private int damagesReceived;

    private float speed;
    private float launchSpeed;
    private float lookAngle;
    private float shovelAttackTimer;
    private float shovelAttackTime;
    private float fireBallTimer;
    private float fireBallTime;
    private float invincibleTimer;
    private float invincibleTime;

    private bool invincible;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 playerPosition;
    private Vector2 lookDirection;

    private Rigidbody2D playerBody = new Rigidbody2D();
    private SpriteRenderer spriteRenderer = new SpriteRenderer();

    private Shovel shovel = new Shovel();

    void Start()
    {
        this.ResetScriptable();
        shovel = this.gameObject.GetComponentInChildren<Shovel>();
        playerBody = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        playerPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection *= speed * Time.deltaTime;

        playerPosition = this.transform.position;
        playerPosition += moveDirection;
        this.transform.position = playerPosition;
    }

    void Update()
    {
        this.UpdateValues();
        this.Attacks();
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        FaceMouse();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScriptable();
        }
        damagesReceived = GameManager.Instance.SendDamages();
        if (damagesReceived != 0 && !invincible)
        {
            this.TakeDamages();
        }
        if (invincible)
        {
            InvokeRepeating("InvincibleClipping", 0.0f, invincibleTime);
        }
    }

    private enum Direction
    {
        Front,
        Back,
        Right,
        Left,
    }

    private void ChangeSprite(Direction direction)
    {
        switch(direction)
        {
            case Direction.Front:
                this.spriteRenderer.sprite = backSprite;
                break;
            case Direction.Back:
                this.spriteRenderer.sprite = frontSprite;
                break;
            case Direction.Right:
                this.spriteRenderer.sprite = sideSprite;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Direction.Left:
                this.spriteRenderer.sprite = sideSprite;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
        }
    }

    private void FaceMouse()
    {
        if (IsBetween(lookAngle, -45, 45))
        {
            ChangeSprite(Direction.Right);
        }
        else if (IsBetween(lookAngle, 45, 135))
        {
            ChangeSprite(Direction.Front);
        }
        else if (IsBetween(lookAngle, -135, -45))
        {
            ChangeSprite(Direction.Back);
        }
        else if (IsBetween(lookAngle, -180, -135) || (IsBetween(lookAngle, 135, 180)))
        {
            ChangeSprite(Direction.Left);
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }

    private void TakeDamages()
    {
        playerValues.HpValue -= damagesReceived;
        invincibleTimer = invincibleTime;
        Invincible();
        invincible = true;
        if (HP <= 0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            Destroy(this.gameObject);
        }
    }

    private void Invincible()
    {
        StartCoroutine(Invincible(invincibleTime));
    }

    IEnumerator Invincible(float hurtTime)
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);


        yield return new WaitForSeconds(hurtTime);

        CancelInvoke();
        this.spriteRenderer.enabled = true;
        invincible = false;
        GameManager.Instance.DamagePlayer(0);
        Physics2D.IgnoreLayerCollision(8, 10, false);

    }

    private void InvincibleClipping()
    {
        if (this.spriteRenderer.enabled)
        {
            this.spriteRenderer.enabled = false;
        }
        else
        {
            this.spriteRenderer.enabled = true;
        }
    }

    private void Attacks()
    {
        if (shovelAttackTimer == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                shovel.Activate();
                shovelAttackTimer = shovelAttackTime;
            }
        }
        if (fireBallTimer == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.LaunchFireBall();
                fireBallTimer = fireBallTime;
            }
        }
        fireBallTimer = Mathf.Clamp(fireBallTimer - Time.deltaTime, 0, fireBallTime);
        shovelAttackTimer = Mathf.Clamp(shovelAttackTimer - Time.deltaTime, 0, shovelAttackTime);
    }

    private void UpdateValues()
    {
        HP = playerValues.HpValue;
        speed = playerValues.speed;
        shovelDamages = playerValues.shovelDamages;
        fireBallDamages = playerValues.fireBallDamages;
        launchSpeed = playerValues.fireBallLaunchSpeed;
        shovelAttackTime = playerValues.shovelCooldown;
        fireBallTime = playerValues.fireBallCooldown;
        invincibleTime = playerValues.invincibleTime;
    }


    private void LaunchFireBall()
    {
        GameObject launchedFireBall = Instantiate(fireBall, this.transform.position, Quaternion.identity);
        lookDirection = (lookDirection.normalized * launchSpeed);
        launchedFireBall.GetComponent<Rigidbody2D>().velocity = lookDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            switch(collision.name)
            {
                case string flesh when flesh.Contains("Flesh"):
                    playerValues.fleshCount++;
                    break;
                case string bones when bones.Contains("Bone"):
                    playerValues.bonesCount++;
                    break;
                case string ectoplasm when ectoplasm.Contains("Ectoplasm"):
                    playerValues.ectoplasmCount++;
                    break;
                case string slime when slime.Contains("Slime"):
                    playerValues.slimeCount++;
                    break;
                default:
                    Debug.Log(collision.name + " not recognized as item");
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    private void ResetScriptable()          // A supprimer au build, sert à reset les valeurs du scriptable aux valeurs par défaut 
    {
        playerValues.HpValue = 10;
        playerValues.speed = 5;
        playerValues.fireBallDamages = 2;
        playerValues.fireBallLaunchSpeed = 7;
        playerValues.fireBallCooldown = 1.5f;
        playerValues.shovelDamages = 3;
        playerValues.shovelCooldown = 1;
        playerValues.fleshCount = 0;
        playerValues.bonesCount = 0;
        playerValues.slimeCount = 0;
        playerValues.ectoplasmCount = 0;
        playerValues.invincibleTime = 1;
    }
}
