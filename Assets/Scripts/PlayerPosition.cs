using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerCtrl _player = GameObject.FindObjectOfType<PlayerCtrl>();
        if (_player != null) { _player.transform.position = transform.position; }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
