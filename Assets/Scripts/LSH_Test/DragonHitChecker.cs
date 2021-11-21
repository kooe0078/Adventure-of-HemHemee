using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHitChecker : MonoBehaviour
{
    public GameObject Dragon;
    private GameObject Player;
    public AudioClip takeDamageSound;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider coll)
    {
        if (Player.GetComponent<PlayerCtrl>()._isAttack == true && coll.gameObject.tag == "Melee")
        {
            Dragon.GetComponent<AudioSource>().PlayOneShot(takeDamageSound);
            Dragon.GetComponent<DragonCtrl>().hp -= coll.gameObject.GetComponent<Weapon>().m_Damage;
            Player.GetComponent<PlayerCtrl>()._isAttack = false;

            if (Dragon.GetComponent<DragonCtrl>().hp <= 0)
            {
                Dragon.GetComponent<DragonCtrl>().MonsterDie();
            }
            else
            {
                Dragon.GetComponent<Animator>().SetTrigger("IsHit");
            }
        }
        if (coll.gameObject.tag == "active")
        {
            Debug.Log("마법공격을 받았습니다");
            Dragon.GetComponent<DragonCtrl>().hp -= coll.GetComponent<ActiveItem>().damage;
            Destroy(coll.gameObject);
        }
    }
}
