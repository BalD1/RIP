using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    [SerializeField]
    private GameObject fireBall;
    [SerializeField]
    private GameObject fireballInstanceCreation;

    [SerializeField] private Canvas interactionButton;

    private string whereIsLooking;

    private int HP;
    private int shovelDamages;
    private int fireBallDamages;
    private int damagesReceived;

    private float speed;
    private float launchSpeed;
    private float lookAngle;
    private float shovelAttackTimer;
    private float shovelAttackTime;
    private float invincibleTimer;
    private float invincibleTime;
    private float fireBallCooldown;

    private float shovelAttackAnimationTime;
    private float fireballAttackAnimationTime;
    private float fireballAttackAnimationTimer;
    private float delayedLaunch;

    private bool invincible;
    private bool canLaunchFireBall;
    private bool healFlag;

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
        GameManager.Instance.SetGameState(GameManager.GameState.InGame); // TEST ONLY
        this.playerState = PlayerState.Idle;
        this.InstantiateStats();
        shovel = this.gameObject.GetComponentInChildren<Shovel>();
        playerBody = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        playerPosition = this.transform.position;
        this.playerAnimator = this.GetComponent<Animator>();
        canLaunchFireBall = true;
        shovelAttackAnimationTime = GameManager.Instance.GetAnimationTimes(playerAnimator, "SBack");
        fireballAttackAnimationTime = GameManager.Instance.GetAnimationTimes(playerAnimator, "FBack");
    }

    private void FixedUpdate()
    {
        this.Movement();
    }

    private void InstantiateStats()
    {
        playerValues.level = 1;
        playerValues.xpNeeded = 150;
        playerValues.xpAmount = 0;
        playerValues.maxHP = playerValues.baseMaxHP;
        playerValues.shovelDamages = playerValues.baseShovelDamages;
        playerValues.fireBallDamages = playerValues.baseFireballDamages;
        playerValues.speed = playerValues.baseSpeed;
        playerValues.invincibleTime = playerValues.baseInvincibleTime;
        playerValues.fireBallCooldown = playerValues.baseFireballCooldown;
        playerValues.shovelCooldown = playerValues.baseShovelCooldown;
        playerValues.fleshCount = 0;
        playerValues.bonesCount = 0;
        playerValues.slimeCount = 0;
        playerValues.ectoplasmCount = 0;
        playerValues.flowerCount = 0;

        this.HP = playerValues.baseMaxHP;
        this.shovelDamages = playerValues.baseShovelDamages;
        this.fireBallDamages = playerValues.baseFireballDamages;
        this.speed = playerValues.baseSpeed;
        this.invincibleTime = playerValues.baseInvincibleTime;
        this.fireBallCooldown = playerValues.baseFireballCooldown;
        this.shovelAttackTime = playerValues.baseShovelCooldown;
    }
    

    void Update()
    {
        GameManager.Instance.PlayerPosition = this.transform.position;
        Debug.Log("Current time : " + GameManager.Instance.SendGameTime() + ". Press Enter to change.");
        this.ChangePlayerMode();        // Change the player mode by time
        this.ChangeAnimation();         // Change the animation following the player's state
        this.FaceMouse();
        this.UpdateValues();
        this.HealAtDay();
        this.GetExperienceAmount();
        

        if (this.playerMode == PlayerMode.Fight)
        {
            this.Attacks();
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
        this.Tests();

        if (canLaunchFireBall)
        {
            fireBallCooldown = playerValues.fireBallCooldown;
        }
        else
        {
            this.FireballCooldown();
        }

        if (GameManager.Instance.PlayerCanInteract)
        {
            this.interactionButton.gameObject.SetActive(true);
        }
        else
        {
            this.interactionButton.gameObject.SetActive(false);
        }

        if (this.interactionButton.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.PlayerInteracted = true;
            }
        }

        if (fireballAttackAnimationTimer > 0)
        {
            fireballAttackAnimationTimer = Mathf.Clamp(fireballAttackAnimationTimer - Time.deltaTime, 0, fireballAttackAnimationTime);
            if (fireballAttackAnimationTimer == 0)
            {
                if (this.playerBody.velocity == Vector2.zero)
                {
                    playerAnimator.SetBool("Idle", true);
                }
                else
                {
                    playerAnimator.SetBool("Idle", false);
                }
                playerAnimator.SetBool("FireballAttacking", false);
            }
        }
        
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && !GameManager.Instance.MouseIsOverSomething)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                UIManager.Instance.BuildDisplayActive = false;
            }
        }
        
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
        Debug.Log("level : " + playerValues.level + "____________" + "xp : " + playerValues.xpAmount + " / " + playerValues.xpNeeded + 
            "____________" + " dégats S : " + playerValues.shovelDamages + "____________" + " dégats F : " + playerValues.fireBallDamages + "____________" + " hp : " + playerValues.maxHP);

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.Instance.ExperienceToPlayer = 100;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameManager.Instance.ExperienceToPlayer = 1000;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.ExperienceToPlayer = 5000;
        }
        if (GameManager.Instance.ExperienceToPlayer != 0)
        {
            Debug.Log(" received : " + GameManager.Instance.ExperienceToPlayer);
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
            playerAnimator.SetFloat("DirectionX", moveDirection.normalized.x);
            playerAnimator.SetFloat("DirectionY", moveDirection.normalized.y);
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
        if (this.playerState != PlayerState.ShovelAttacking && GameManager.Instance.SendGameState() == GameManager.GameState.InGame)
        {
            playerAnimator.SetFloat("Look X", clampedLookDirection.x);
            playerAnimator.SetFloat("Look Y", clampedLookDirection.y);
            playerAnimator.SetFloat("Speed", moveDirection.magnitude);
        }

        if (IsBetween(lookAngle, -45, 45))
        {
            // right
            whereIsLooking = "right";
        }
        else if (IsBetween(lookAngle, 45, 135))
        {
            // front
            whereIsLooking = "front";
        }
        else if (IsBetween(lookAngle, -135, -45))
        {
            // back
            whereIsLooking = "back";
        }
        else if (IsBetween(lookAngle, -180, -135) || (IsBetween(lookAngle, 135, 180)))
        {
            // left
            whereIsLooking = "left";
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }

    private void ChangeAnimation()
    {
        if (canLaunchFireBall)
        {
            playerAnimator.SetBool("Lit", true);
        }
        else
        {
            playerAnimator.SetBool("Lit", false);
        }

        if (playerState == PlayerState.Idle)
        {
            playerAnimator.SetBool("Idle", true);
        }
        else
        {
            playerAnimator.SetBool("Idle", false);
        }
    }

    // ----------------------- Player taking damages and invincibility ------------------------

    private void TakeDamages()
    {
        playerValues.HpValue -= damagesReceived;
        HP = playerValues.HpValue;
        if (HP <= 0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            playerAnimator.SetBool("Death", true);
            Destroy(this.gameObject);
        }
        invincibleTimer = invincibleTime;
        Invincible();
        invincible = true;

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

    private void HealAtDay()
    {
        if(GameManager.Instance.SendGameTime() == GameManager.GameTime.Day && !healFlag)
        {
            healFlag = true;
            playerValues.HpValue = playerValues.maxHP;
        }
        else if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night)
        {
            healFlag = false;
        }
    }

    // ------------------------ Player attacks --------------------------

    private void Attacks()
    {
        if (shovelAttackTimer == 0 && playerState != PlayerState.ShovelAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                shovel.Activate(whereIsLooking);
                this.playerState = PlayerState.ShovelAttacking;
                playerAnimator.SetBool("ShovelAttacking", true);
                shovelAttackTimer = shovelAttackTime;
            }
        }
        if (canLaunchFireBall)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerAnimator.SetBool("FireballAttacking", true);
                Invoke("LaunchFireBall", fireballAttackAnimationTime / 2);
                fireballAttackAnimationTimer = fireballAttackAnimationTime;
                canLaunchFireBall = false;
            }
        }
        shovelAttackTimer = Mathf.Clamp(shovelAttackTimer - Time.deltaTime, 0, shovelAttackTime);
        if (shovelAttackTimer == 0 && this.playerState == PlayerState.ShovelAttacking)
        {
            this.playerState = PlayerState.Idle;
            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("ShovelAttacking", false);
        }
    }

    private void LaunchFireBall()
    {
        GameManager.Instance.PlayerLookAngle = lookAngle;
        GameObject launchedFireBall = Instantiate(fireBall, fireballInstanceCreation.transform.position, Quaternion.identity);
        lookDirection = (lookDirection.normalized * launchSpeed);
        launchedFireBall.GetComponent<Rigidbody2D>().velocity = lookDirection;
    }

    private void FireballCooldown()
    {
        fireBallCooldown = Mathf.Clamp(fireBallCooldown - Time.deltaTime, 0, fireBallCooldown);
        if (fireBallCooldown == 0)
        {
            canLaunchFireBall = true;
            fireBallCooldown = playerValues.fireBallCooldown;
        }
    }

    // ------------------------ Player Level ---------------------------

    private void GetExperienceAmount()
    {
        int receivedXP = GameManager.Instance.ExperienceToPlayer;
        if (receivedXP > 0)
        {
            playerValues.xpAmount += receivedXP;
            if (playerValues.xpNeeded <= playerValues.xpAmount)
            {
                receivedXP -= playerValues.xpNeeded;
                playerValues.xpAmount -= playerValues.xpNeeded;
                GameManager.Instance.ExperienceToPlayer = receivedXP;
                LevelUP();
            }
            else
            {
                GameManager.Instance.ExperienceToPlayer = 0;
            }
        }
    }

    private void LevelUP()
    {
        playerValues.level++;
        playerValues.xpNeeded = 25 * playerValues.level * (1 + playerValues.level);
        playerValues.maxHP = Mathf.FloorToInt( (float) ( (playerValues.level - 1) + (playerValues.maxHP - (playerValues.maxHP * 0.9)) ) * 
                                              (float) ( (playerValues.level - 1) - ((playerValues.level - 1) * 0.9) ) + playerValues.baseMaxHP );

        playerValues.shovelDamages = Mathf.FloorToInt((playerValues.shovelDamages + (playerValues.level / 2.5f)) - (playerValues.level / playerValues.baseShovelDamages));

        playerValues.fireBallDamages = Mathf.RoundToInt((playerValues.fireBallDamages + (playerValues.level / 2)) - (playerValues.level / 2.5f));
        playerValues.HpValue = playerValues.maxHP;
        UIManager.Instance.ChangeLevelDisplay();
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
                case string flower when flower.Contains("flower"):
                    playerValues.flowerCount++;
                    break;
                default:
                    Debug.Log(collision.name + " not recognized as item");
                    break;
            }
        }
        Torches torch = collision.GetComponent<Torches>();
        if (torch != null)
        {
            if(torch.IsLighted())
            {
                this.canLaunchFireBall = true;
            }
        }
    }
    
}
