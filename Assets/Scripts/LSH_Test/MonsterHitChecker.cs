using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitChecker : MonoBehaviour
{
    public GameObject Monster;
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
            Monster.GetComponent<MonsterCtrl>().hp -= coll.gameObject.GetComponent<Weapon>().m_Damage;
            Player.GetComponent<PlayerCtrl>()._isAttack = false;

            if (Monster.GetComponent<MonsterCtrl>().hp <= 0)
            {
                Monster.GetComponent<MonsterCtrl>().MonsterDie();
            }
            else
            {
                Monster.GetComponent<Animator>().SetTrigger("IsHit");
            }
        }
        if(coll.gameObject.tag == "active")
        {
            Debug.Log("마법공격을 받았습니다");
            Monster.GetComponent<MonsterCtrl>().hp -= coll.GetComponent<ActiveItem>().damage;
            Destroy(coll.gameObject);
        }
    }
}
