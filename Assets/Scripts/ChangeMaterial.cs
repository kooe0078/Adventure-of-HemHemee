using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    // 메테리얼 종류 활성화
    [SerializeField] protected Material[] material;
    Renderer render;
    int m_Material;


    public void Changematerial()
    {
        // 메테리얼의 id
        m_Material = Random.Range(0, material.Length);
        if (gameObject.activeSelf == true)
        {
            // 렌더는 m_Material로 변환시킴
            render = gameObject.GetComponent<Renderer>();
            render.sharedMaterial = material[m_Material];
        }
    }

}
