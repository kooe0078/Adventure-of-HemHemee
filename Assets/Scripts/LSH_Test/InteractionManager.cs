using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private GameObject Player;
    public GameObject Text;
    public bool interactionChecker = true;

    // 아이템을 넣어줄 배열
    [SerializeField] GameObject[] randomItem = null;
    // 랜덤 값이 들어갈 변수
    int randomCount = 0;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Text.gameObject.SetActive(true);
        }
    }
    public void OnTriggerStay(Collider coll)
    {
        if (interactionChecker == true && coll.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("제단을 사용합니다");
            ItemSpwan();
            interactionChecker = false;
        }
        if (interactionChecker == false && coll.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("이미 사용한 제단입니다");
        }
    }
    public void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Text.gameObject.SetActive(false);
            interactionChecker = true;
        }
    }
    public void ItemSpwan()
    {
        randomCount = Random.Range(0, randomItem.Length);
        Instantiate(randomItem[randomCount], 
            new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z - 1), Quaternion.identity);
    }
}
