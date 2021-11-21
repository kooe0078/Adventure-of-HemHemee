using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitChecker : MonoBehaviour
{
    public GameObject Golem;
    private GameObject Player;
    //public AudioClip takeDamageSound;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider coll)
    {
        if (Player.GetComponent<PlayerCtrl>()._isAttack == true && coll.gameObject.tag == "Melee")
        {
            //Golem.GetComponent<AudioSource>().PlayOneShot(takeDamageSound);
            Golem.GetComponent<GolemCtrl>().hp -= coll.gameObject.GetComponent<Weapon>().m_Damage;
            Player.GetComponent<PlayerCtrl>()._isAttack = false;

            if (Golem.GetComponent<GolemCtrl>().hp <= 0)
            {
                Golem.GetComponent<GolemCtrl>().MonsterDie();
            }
            else
            {
                Golem.GetComponent<Animator>().SetTrigger("IsHit");
            }
        }
        if (coll.gameObject.tag == "active")
        {
            Debug.Log("마법공격을 받았습니다");
            Golem.GetComponent<GolemCtrl>().hp -= coll.GetComponent<ActiveItem>().damage;
            Destroy(coll.gameObject);
        }
    }
}
