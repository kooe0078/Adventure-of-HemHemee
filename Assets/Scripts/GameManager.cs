using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 SpawnPoint;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject _player = null;
        if (GameObject.FindObjectOfType<PlayerCtrl>() == null)
        { _player = Instantiate(player, SpawnPoint, Quaternion.identity); }
        else
            _player = GameObject.FindObjectOfType<PlayerCtrl>().gameObject;
        DontDestroyOnLoad(_player);

        var gameover = player.GetComponent<PlayerCtrl>();
       gameover._gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
