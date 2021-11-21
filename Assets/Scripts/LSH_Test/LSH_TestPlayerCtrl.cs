using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSH_TestPlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr;
    public float moveSpeed = 10.0f;
    public int hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "MonsterAttack")
        {
            hp -= 5;
            Debug.Log("Player HP = " + hp.ToString());
            // 플레이어의 체력이 0 이하면 사망처리
            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!!");
        //MONSTER이라는 태그의 게임 오브젝트를 모두 찾음
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        // 모든 몬스터의 OnPlayerDie 함수를 순차적 호출
        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }
}
