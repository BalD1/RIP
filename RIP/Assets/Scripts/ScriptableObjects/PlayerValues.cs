using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ValuesHolder")]
public class PlayerValues : ScriptableObject
{
    public int HpValue;
    public float speed;

    public int fireBallDamages;
    public float fireBallLaunchSpeed;
    public float fireBallCooldown;

    public int shovelDamages;
    public float shovelCooldown;
}
