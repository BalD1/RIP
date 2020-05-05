using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    public GameObject Ennemy;
    private Vector2 tombPos;
    private bool Spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        tombPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Night && Spawned == false)
        {
            Instantiate(Ennemy, tombPos, Quaternion.identity);
            Spawned = true;
        }
        if (GameManager.Instance.SendGameTime() == GameManager.GameTime.Day)
        {
            Spawned = false;
        }
    }
}
