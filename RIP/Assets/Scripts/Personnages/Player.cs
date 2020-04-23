using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    [SerializeField]
    GameObject fireBall;

    private int HP;
    private int shovelDamages;
    private int fireBallDamages;

    private float speed;
    private float launchSpeed;
    private float lookAngle;
    private float shovelAttackTimer;
    private float shovelAttackTime;
    private float fireBallTimer;
    private float fireBallTime;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 playerPosition;
    private Vector2 lookDirection;

    private Rigidbody2D playerBody = new Rigidbody2D();

    Shovel shovel = new Shovel();

    void Start()
    {
        shovel = this.gameObject.GetComponentInChildren<Shovel>();
        playerBody = this.GetComponent<Rigidbody2D>();
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
        Debug.Log(playerValues.fleshCount);
        Debug.Log(playerValues.bonesCount);
        Debug.Log(playerValues.ectoplasmCount);
        Debug.Log(playerValues.slimeCount);
        this.UpdateValues();
        this.Attacks();
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        FaceMouse();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScriptable();
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
    }

    private void FaceMouse()
    {
        if (lookDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
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
    }
}
