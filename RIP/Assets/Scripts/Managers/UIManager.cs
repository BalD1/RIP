using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private Player player = new Player();
    private Text comptItems;
    private List<GameObject[]> itemsList;
    private GameObject[] boneList;
    private GameObject[] zombList;
    private BoxCollider2D boneCollider;

    private int comptOs = 0;
    private int comptZomb = 0;
    int indexz;
    int indexb;



    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Ca par exemple, l'instance du UI Manager est nulle :o");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
       // player = GetComponent<BoxCollider2D>();
        boneList = GameObject.FindGameObjectsWithTag("Bone");
        zombList = GameObject.FindGameObjectsWithTag("Zomb");
        itemsList.Add(boneList);
        itemsList.Add(zombList);
        comptItems = GetComponent<Text>();
    }

    private void Start()
    {

    }

    void Update()
    {
        if(player)




        if (Input.GetKeyDown(KeyCode.N))
        {

            if (indexb == boneList.Length)
            {
                return;
            }
            else
            {
                    Compteur(itemsList, zombList, boneList[indexb], ref comptOs, ref indexb);
                indexb++;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {

            if (indexz == zombList.Length)
            {
                return;
            }
            else
            {
                Compteur(itemsList, zombList, zombList[indexz], ref comptZomb, ref indexz);
                indexz++;
            }
        }

        comptItems.text = "Os : " + comptOs.ToString() + System.Environment.NewLine + "Zomb : " + comptZomb.ToString();
    }


    private int Compteur(List<GameObject[]> itemsBigList, GameObject[] itemsList, GameObject items, ref int comp, ref int i)
    {
    //    itemsBigList.Remove(itemsList[items]);
        comp++;
        return comp;
    }
}
