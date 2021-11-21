using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject Boss;
    public Image attackImage;
    public Image ActiveImage;
    public Image HpBar;
    public Image SteminaBar;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI AttackSpeedText;

    public Image BossHpBar;


    public float canAttack;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }
    // Update is called once per frame
    void Update()
    {
        canAttack = player.GetComponent<PlayerCtrl>().canAttack;
        attackImage.GetComponent<Image>().fillAmount = canAttack;
      
         DamageText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerCtrl>().m_EquipWeapon.m_Damage + "";

        AttackSpeedText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerCtrl>().m_EquipWeapon.m_AttackSpeed + "";

        ActiveImage.GetComponent<Image>().fillAmount = 1 - (player.GetComponent<PlayerCtrl>().m_currentActiveCooltime / player.GetComponent<PlayerCtrl>().m_MaxActive_Cooltime);


        if (Boss != null)
        {
            BossHpBar.GetComponent<Image>().fillAmount = (float)Boss.GetComponent<BossCtrl>().hp / (float)Boss.GetComponent<BossCtrl>().maxHp;
        }
        HpText.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(player.GetComponent<PlayerCtrl>().HP) + "/" + Mathf.Floor(player.GetComponent<PlayerCtrl>().maxHP);
        if (player.GetComponent<PlayerCtrl>().HP != 0)
        {
            HpBar.GetComponent<Image>().fillAmount = player.GetComponent<PlayerCtrl>().HP / player.GetComponent<PlayerCtrl>().maxHP;
        }
        else
            HpBar.GetComponent<Image>().fillAmount = 0;

        if (player.GetComponent<PlayerCtrl>().m_currentStamina != 0)
        {
            SteminaBar.fillAmount = player.GetComponent<PlayerCtrl>().m_currentStamina / player.GetComponent<PlayerCtrl>().m_MaxStamina;
        }
    }
}