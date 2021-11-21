using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Coin, HP, Weapon, Accessory, Stat };
    public Type type;
    public int value;
    public int HpVaule;
    public int DamageValue;
    public float AttackSpeedValue;
    public float StatValue;
    public GameObject WeaponEffects;

    void Update()
    {
        transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
    }
}
