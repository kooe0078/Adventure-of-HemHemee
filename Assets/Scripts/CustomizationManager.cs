using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    // 캐릭터에 존재하는 파츠
    enum CharacterParts
    {
        ACCESSORY_MODEL,
        COUSTUME_MODEL,
        EYE_MODEL,
        HAIR_MODEL
    }

    [SerializeField] protected GameObject[] hairModels;
    [SerializeField] protected GameObject[] coustomModels;
    [SerializeField] protected GameObject[] eyeModels;
    [SerializeField] protected GameObject[] accessoryModels;


    int hairIndex = 0;
    int coustomIndex = 0;
    int eyeIndex = 0;
    int accessoryIndex = 0;

    public int m_HpPoint;
    public int m_DamagePoint;
    public double m_AttackSpeedPoint;

    private void Start()
    {
        RandomModel();
    }
    public void RandomModel()
    {
        m_HpPoint = Random.Range(80, 120);
        m_DamagePoint = Random.Range(5, 10);
        m_AttackSpeedPoint = Random.Range(5, 11) * 0.1;


        // 각 모델의 범위 지정
        hairIndex = Random.Range(0, hairModels.Length);
        coustomIndex = Random.Range(0, coustomModels.Length);
        eyeIndex = Random.Range(0, eyeModels.Length);
        accessoryIndex = Random.Range(0, accessoryModels.Length);

        ApplyModification(CharacterParts.HAIR_MODEL, hairIndex);
        ApplyModification(CharacterParts.COUSTUME_MODEL, coustomIndex);
        ApplyModification(CharacterParts.EYE_MODEL, eyeIndex);
        ApplyModification(CharacterParts.ACCESSORY_MODEL, accessoryIndex);
    }


    // 캐릭터에 랜덤한 프리셋을 저장시킴
    void ApplyModification(CharacterParts parts, int id)
    {


        switch (parts)
        {
            case CharacterParts.HAIR_MODEL:
                for (int i = 0; i < hairModels.Length; i++)
                    hairModels[i].gameObject.SetActive(false);
                hairModels[id].gameObject.SetActive(true);
                break;
            case CharacterParts.COUSTUME_MODEL:
                for (int i = 0; i < coustomModels.Length; i++)
                    coustomModels[i].gameObject.SetActive(false);
                coustomModels[id].gameObject.SetActive(true);
                break;
            case CharacterParts.EYE_MODEL:
                for (int i = 0; i < eyeModels.Length; i++)
                    eyeModels[i].gameObject.SetActive(false);
                eyeModels[id].gameObject.SetActive(true);
                break;
            case CharacterParts.ACCESSORY_MODEL:
                for (int i = 0; i < accessoryModels.Length; i++)
                    accessoryModels[i].gameObject.SetActive(false);
                accessoryModels[id].gameObject.SetActive(true);
                break;
        }
    }
    // 플레이어의 캐릭터 정보 저장
    public void SaveCharacter()
    {
        GameObject characterPreset = GameObject.Find("Player");
        characterPreset.GetComponent<PlayerCtrl>().GameStart();
        characterPreset.GetComponent<PlayerCtrl>().SetStat(m_HpPoint, m_DamagePoint, m_AttackSpeedPoint);
        characterPreset.GetComponent<PlayerCtrl>().HP = characterPreset.GetComponent<PlayerCtrl>().maxHP;
        GameObject Player = PrefabUtility.SaveAsPrefabAsset(characterPreset, "Assets/Prefabs/Player.prefab");
    }
}