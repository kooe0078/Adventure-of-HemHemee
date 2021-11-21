using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    public GameObject CustomizeManager;

    public TextMeshProUGUI m_HpPoint;
    public TextMeshProUGUI m_DamagePoint;
    public TextMeshProUGUI m_AttackSpeedPoint;

    // Update is called once per frame
    void Update()
    {

        m_HpPoint.GetComponent<TextMeshProUGUI>().text = CustomizeManager.GetComponent<CustomizationManager>().m_HpPoint + "";
        m_DamagePoint.GetComponent<TextMeshProUGUI>().text = CustomizeManager.GetComponent<CustomizationManager>().m_DamagePoint + "";
        m_AttackSpeedPoint.GetComponent<TextMeshProUGUI>().text = CustomizeManager.GetComponent<CustomizationManager>().m_AttackSpeedPoint + "";
    }
}