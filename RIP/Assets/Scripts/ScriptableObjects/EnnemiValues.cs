using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnnemiValues", menuName = "ValuesHolderEnnemi")]
public class EnnemiValues : ScriptableObject
{
    public int zombieHp;
    public float zombieSpd;
    public int zombieAtk;

    public int squeletteHp;
    public float squeletteSpd;
    public int squeletteAtk;

    public int slimeHp;
    public float slimeSpd;
    public int slimeAtk;

    public int fantômeHp;
    public float fantômeSpd;
    public int fantômeAtk;
}
