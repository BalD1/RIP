using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;
    [SerializeField] private float maxDistance;

    private int damages;

    private Vector2 originalPosition;

    private float playerLookAngle;

    void Start()
    {
        playerLookAngle = GameManager.Instance.PlayerLookAngle;
        Vector3 rotationAngle = Vector3.zero;
        rotationAngle.z = playerLookAngle;
        this.transform.eulerAngles = rotationAngle;
        damages = playerValues.fireBallDamages;
        originalPosition = this.transform.position;
    }
    
    void Update()
    {
        if (Vector2.Distance(originalPosition, this.transform.position) > maxDistance)       // Détruit le spell si assez loin
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.gameObject.name + " touche " + collision + " et lui inflige " + damages);
        GameManager.Instance.DamageEnnemi(damages);
        Destroy(this.gameObject);
    }

}
