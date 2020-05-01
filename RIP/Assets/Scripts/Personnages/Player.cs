using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    [SerializeField]
    private GameObject fireBall;

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
    private Animator playerAnimator;

    private PlayerState playerState;
    private PlayerMode playerMode;

    private Shovel shovel;

    private enum PlayerState
    {
        Idle,
        Moving,
        ShovelAttacking,
        FireballAttacking,
        Dead,
    }

    private enum PlayerMode
    {
        Build,
        Fight,
    }

    // --------------------- Start & Updates ------------------

    void Start()
    {
        this.playerState = PlayerState.Idle;
        this.ResetScriptable();
        shovel = this.gameObject.GetComponentInChildren<Shovel>();
        playerBody = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        playerPosition = this.transform.position;
        this.playerAnimator = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        this.Movement();
    }
    

    void Update()
    {
        Debug.Log("Current time : " + GameManager.Instance.SendGameTime() + ". Press Enter to change.");
        this.UpdateValues();
        this.ChangePlayerMode();        // Change the player mode by time

        if (this.playerMode == PlayerMode.Fight)
        {
            this.Attacks();
        }

        FaceMouse();

        damagesReceived = GameManager.Instance.SendDamages();
        if (damagesReceived != 0 && !invincible)
        {
            this.TakeDamages();
        }
        if (invincible)
        {
            InvokeRepeating("InvincibleClipping", 0.0f, invincibleTime);
        }
        this.Tests();
    }

    private void ChangePlayerMode()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            this.playerMode = PlayerMode.Build;
        }
        else
        {
            this.playerMode = PlayerMode.Fight;
        }
    }

    // ----------------------- Code tests -------------------

    private void Tests()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScriptable();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (this.playerMode == PlayerMode.Build)
            {
                GameManager.Instance.SetGameTime(GameManager.GameTime.Night);
            }
            else
            {
                GameManager.Instance.SetGameTime(GameManager.GameTime.Day);
            }
        }
    }


    // ---------------------- Movement and look direction ----------------

    private void Movement()
    {
        if (this.playerState == PlayerState.Idle ^ this.playerState == PlayerState.Moving)
        {
            moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveDirection *= speed * Time.deltaTime;
            if (moveDirection != Vector2.zero)
            {
                this.playerState = PlayerState.Moving;
            }
            else
            {
                this.playerState = PlayerState.Idle;
            }
            playerPosition = this.transform.position;
            playerPosition += moveDirection;
            this.transform.position = playerPosition;
        }
    }

    private enum Direction
    {
        Front,
        Back,
        Right,
        Left,
    }

    private void FaceMouse()                    // Gets the mouse position and makes the player look at it
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Animator variables
        Vector2 clampedLookDirection = new Vector2();
        clampedLookDirection.x = Mathf.Clamp(lookDirection.x, -5f, 5f);
        clampedLookDirection.y = Mathf.Clamp(lookDirection.y, -5f, 5f);
        playerAnimator.SetFloat("Look X", clampedLookDirection.x);
        playerAnimator.SetFloat("Look Y", clampedLookDirection.y);
        playerAnimator.SetFloat("Speed", moveDirection.magnitude);

        if (IsBetween(lookAngle, -45, 45))
        {
            // right
        }
        else if (IsBetween(lookAngle, 45, 135))
        {
            // front
        }
        else if (IsBetween(lookAngle, -135, -45))
        {
            // back
        }
        else if (IsBetween(lookAngle, -180, -135) || (IsBetween(lookAngle, 135, 180)))
        {
            // left
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }

    // ----------------------- Player taking damages and invincibility ------------------------

    private void TakeDamages()
    {
        playerValues.HpValue -= damagesReceived;
        invincibleTimer = invincibleTime;
        Invincible();
        invincible = true;
        if (HP <= 0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            playerAnimator.SetBool("Death", true);
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

    // ------------------------ Player attacks --------------------------

    private void Attacks()
    {
        if (shovelAttackTimer == 0)
        {
            if (this.playerState == PlayerState.ShovelAttacking)
            {
                this.playerState = PlayerState.Idle;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                shovel.Activate();
                this.playerState = PlayerState.ShovelAttacking;
                shovelAttackTimer = shovelAttackTime;
            }
        }
        if (fireBallTimer == 0)
        {
            if (this.playerState == PlayerState.FireballAttacking)
            {
                this.playerState = PlayerState.Idle;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.LaunchFireBall();
                this.playerState = PlayerState.FireballAttacking;
                fireBallTimer = fireBallTime;
            }
        }
        fireBallTimer = Mathf.Clamp(fireBallTimer - Time.deltaTime, 0, fireBallTime);
        shovelAttackTimer = Mathf.Clamp(shovelAttackTimer - Time.deltaTime, 0, shovelAttackTime);
    }

    private void LaunchFireBall()
    {
        GameObject launchedFireBall = Instantiate(fireBall, this.transform.position, Quaternion.identity);
        lookDirection = (lookDirection.normalized * launchSpeed);
        launchedFireBall.GetComponent<Rigidbody2D>().velocity = lookDirection;
    }

    // ------------------------ Values update --------------------------

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Items pickup
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
