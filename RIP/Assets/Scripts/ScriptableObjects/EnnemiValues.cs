using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnnemiValues", menuName = "ValuesHolderEnnemi")]
public class EnnemiValues : ScriptableObject
{
    public int baseZombieHp;
    public float baseZombieSpd;
    public int baseZombieAtk;
    public int zombieHp;
    public float zombieSpd;
    public int zombieAtk;

    public int squeletteHp;
    public float squeletteSpd;
    public int squeletteAtk;
    public int baseSqueletteHp;
    public float baseSqueletteSpd;
    public int baseSqueletteAtk;

    public int slimeHp;
    public float slimeSpd;
    public int slimeAtk;
    public int baseSlimeHp;
    public float baseSlimeSpd;
    public int baseSlimeAtk;

    public int fantômeHp;
    public float fantômeSpd;
    public int fantômeAtk;
    public int baseFantômeHp;
    public float baseFantômeSpd;
    public int baseFantômeAtk;

    public float invincibleTime;
    public int level;
    public int dropXP;
    public int baseDropXP;

    public void InitializeStats()
    {
        level = 1;
        dropXP = baseDropXP;

        zombieHp = baseZombieHp;
        zombieSpd = baseZombieSpd;
        zombieAtk = baseZombieAtk;

        squeletteHp = baseSqueletteHp;
        squeletteSpd = baseSqueletteSpd;
        squeletteAtk = baseSqueletteAtk;

        slimeHp = baseSlimeHp;
        slimeSpd = baseSlimeSpd;
        slimeAtk = baseSlimeAtk;

        fantômeHp = baseFantômeHp;
        fantômeSpd = baseFantômeSpd;
        fantômeAtk = baseFantômeAtk;
    }
}
