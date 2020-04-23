using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiExemple : EnnemiParent
{
    // Script d'un ennemi type, qu'on pourra copier / coller juste en modifiant les valeurs nécéssaires


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            GameManager.Instance.DamagePlayer(1);
        }
    }
}
