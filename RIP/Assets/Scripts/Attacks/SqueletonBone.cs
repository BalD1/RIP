using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueletonBone : MonoBehaviour //QuentinLamy
{
    private float timer;
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        this.transform.Rotate(0,0,2);
        if(timer >= 4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player joueur = collision.gameObject.GetComponent<Player>();

        if(joueur != null)
        {
            GameManager.Instance.DamagePlayer(1);
        }

        Destroy(this.gameObject);
    }
}
