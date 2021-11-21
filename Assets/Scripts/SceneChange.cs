using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string trasnferMapName; // 이동할 맵의 이름 

    //public GameObject Monster;
    

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Player" /*&& Monster.GetComponent<DragonCtrl>().isDie == true*/)
        {
            SceneManager.LoadScene(trasnferMapName); // 오브젝트에 스크립트 적용 후 원하는 다음 씬 입력하면 충돌 했을 경우 해당 이동함.
        }
    }
  
}
