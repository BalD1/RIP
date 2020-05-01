using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;

    private Vector2 lookDirection;
    private Vector2 originalPosition;

    private PolygonCollider2D thisCollider;

    private Animator shovelAnimator;

    private float shovelAnimTimer;
    private float shovelAnimTime;

    private int damages;

    private void Awake()
    {
        thisCollider = this.GetComponent<PolygonCollider2D>();
        thisCollider.enabled = false;
        shovelAnimator = this.GetComponent<Animator>();
        shovelAnimTime = 0.75f;         // valeur de test en attendant les graphs
        shovelAnimTimer = shovelAnimTime;
    }

    void Start()
    {

    }
    
    void Update()
    {
        damages = playerValues.shovelDamages;
        
        if (thisCollider.isActiveAndEnabled)
        {
            shovelAnimator.Play("shovelAnim");
            shovelAnimTimer = Mathf.Clamp(shovelAnimTimer - Time.deltaTime, 0, shovelAnimTime);
            if (shovelAnimTimer == 0)
            {
                thisCollider.enabled = false;
                shovelAnimTimer = shovelAnimTime;
            }
        }
    }

    public void Activate()
    {
        thisCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.gameObject.name + " touche " + collision + " et lui inflige " + damages);
    }
}
