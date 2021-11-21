using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossSoundManager : MonoBehaviour
{
    // 보스전의 상황에 따른 BGM을 실행하도록 설정
    public AudioClip BossBackGroundSound;
    public AudioClip BossEndBackGroundSound;
    AudioSource bossAudioSource;

    // 다른 스크립트 변수 호출
    GolemCtrl golemCtrl;

    // 한번만 실행 될 플래그
    bool oneTime = true;

    void Start()
    {
        bossAudioSource = GetComponent<AudioSource>();
        golemCtrl = GameObject.Find("Polygonal Golem Green").GetComponent<GolemCtrl>();
    }


    void Update()
    {
        // 보스몬스터 생존시 BGM 재생
        if (golemCtrl.isDie == false && oneTime == true)
        {
            bossAudioSource.clip = BossBackGroundSound;
            bossAudioSource.Play();
            oneTime = false;
        }
        // 보스몬스터 사망시 다른 BGM 재생
        else if (golemCtrl.isDie == true && oneTime == false)
        {
            bossAudioSource.clip = BossEndBackGroundSound;
            bossAudioSource.Play();
            oneTime = true;
        }
    }
}
