using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    public int damage;
    public float speed;
    private void Start()
    {
        damage = GetComponent<Item>().DamageValue;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
