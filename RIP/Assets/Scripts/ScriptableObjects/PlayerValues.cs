using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ValuesHolder")]
public class PlayerValues : ScriptableObject
{
    public int maxHP;
    public int HpValue;
    public float invincibleTime;
    public float speed;

    public int fireBallDamages;
    public float fireBallLaunchSpeed;
    public float fireBallCooldown;

    public int shovelDamages;
    public float shovelCooldown;

    public int fleshCount;
    public int bonesCount;
    public int ectoplasmCount;
    public int slimeCount;
    public int flowerCount;
}
