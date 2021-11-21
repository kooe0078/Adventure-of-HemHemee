using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range, Magic };
    public Type type;
    public int m_Damage;
    public float m_AttackSpeed;
    private GameObject player;
    public GameObject Effects;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public bool Use()
    {
        if (type == Type.Melee)
        {
            if (player.GetComponent<PlayerCtrl>()._isAttack)
            {
                return true;
            }
        }

        return false;
    }
}