using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    private int damages;

    private Vector2 originalPosition;

    void Start()
    {
        damages = playerValues.fireBallDamages;
        originalPosition = this.transform.position;
    }
    
    void Update()
    {
        if(Vector2.Distance(originalPosition, this.transform.position) > 30f)       // Détruit le spell si assez loin
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.gameObject.name + " touche " + collision + " et lui inflige " + damages);
        Destroy(this.gameObject);
    }

}
