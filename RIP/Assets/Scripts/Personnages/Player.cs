using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float speed;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 playerPosition;

    private Rigidbody2D playerBody = new Rigidbody2D();

    void Start()
    {
        speed = 5.0f;
        playerBody = this.GetComponent<Rigidbody2D>();
        playerPosition = this.transform.position;
    }


    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection *= speed * Time.deltaTime;

        playerPosition = this.transform.position;
        playerPosition += moveDirection;
        this.transform.position = playerPosition;


    }
}
