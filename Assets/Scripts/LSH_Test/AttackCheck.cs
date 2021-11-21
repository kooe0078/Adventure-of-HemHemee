using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    public bool inAttackRange = false;

    public void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //Debug.Log("충돌중");
            inAttackRange = true;
        }
    }
    public void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //Debug.Log("충돌해제");
            inAttackRange = false;
        }
    }
}
