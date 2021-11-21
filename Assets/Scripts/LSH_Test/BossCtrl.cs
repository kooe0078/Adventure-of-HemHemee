using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCtrl : MonoBehaviour
{
    // 몬스터의 위치
    protected Transform monsterTr;
    // 플레이어의 위치
    protected Transform playerTr;
    // 네비게이션
    protected NavMeshAgent nvAgent;
    // 애니메이션
    protected Animator animator;

    public int hp;
    public int maxHp = 100;

}
