using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ValuesHolder")]
public class PlayerValues : ScriptableObject
{
    // Levels

    public int level;
    public int xpAmount;
    public int xpNeeded;

    // General stats

    public int baseMaxHP;
    public int maxHP;
    public int HpValue;

    public int baseShovelDamages;
    public int shovelDamages;

    public int baseFireballDamages;
    public int fireBallDamages;
    public float baseFireballLaunchSpeed;
    public float fireBallLaunchSpeed;

    public float baseSpeed;
    public float speed;


    // Cooldowns

    public float baseInvincibleTime;
    public float invincibleTime;

    public float baseFireballCooldown;
    public float fireBallCooldown;
    public float baseShovelCooldown;
    public float shovelCooldown;

    // Items
    
    public int fleshCount;
    public int bonesCount;
    public int ectoplasmCount;
    public int slimeCount;
    public int flowerCount;
}
