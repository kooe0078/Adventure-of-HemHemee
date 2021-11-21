using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    PlayerCtrl playerctrl;
    private Transform EffectTransform;
    public int fel;

    private void Start()
    {
        playerctrl = FindObjectOfType<PlayerCtrl>();
        EffectTransform = GetComponent<Transform>();
        Destroy(this.gameObject, 1.5f);
    }
    private void Update()
    {
        if (fel == 1)
        {
            EffectTransform.position = playerctrl.m_AttackPos1.transform.position;
        }
        if (fel == 2)
        {
            EffectTransform.position = playerctrl.m_AttackPos2.transform.position;
        }
        if (fel == 3)
        {
            EffectTransform.position = playerctrl.m_AttackPos3.transform.position;
        }
        if (fel == 4)
        {
            EffectTransform.position = playerctrl.m_AttackPos4.transform.position;
        }
    }
}