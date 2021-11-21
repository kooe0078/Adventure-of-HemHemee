using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    // 바라볼 대상
    public GameObject player;
    public Transform target;

    void Update()
    {
        // 지속적으로 타겟의 방향을 바라보는 코드
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        Vector3 vec = target.position - transform.position;
        vec.Normalize();
        Quaternion quaternion = Quaternion.LookRotation(vec);
        transform.rotation = quaternion;
    }
}
