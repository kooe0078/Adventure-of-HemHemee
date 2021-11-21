using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GolemCtrl : BossCtrl
{
    // 몬스터의 상태 설정
    public enum GolemState { idle, run, attack, groundAttack, doublePunch, die };
    public GolemState golemState = GolemState.idle;

    // 추적 범위, 공격 거리, 사망 여부 확인, 몬스터 체력
    public float traceDist = 50.0f;
    public float attackDist = 7.0f;
    public bool isDie = false;

    // 몬스터가 공격하는 플래그, 타이머
    bool pattenOn = false;
    bool pattenType = false; // true일때 땅찍기, false일때 스펠 캐스트
    public float attackTime = 0.0f;
    public float pattenTime = 0.0f;

    // 몬스터의 패턴 폭발 이펙트, 위치
    public GameObject explosionObject;
    [SerializeField] Transform[] explosionPos = null;

    // 어택박스 체크 코드 호출
    AttackCheck attackChecking;

    void Start()
    {

        hp = maxHp;
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(this.CheckGolemState());
        StartCoroutine(this.GolemAction());

        // 플레이어가 몬스터의 앞에 있는지 체크하는 변수값 가져오기
        attackChecking = GameObject.Find("AttackPoint").GetComponent<AttackCheck>();
    }

    void Update()
    {
        // 골렘의 공격 패턴 조정 (8초동안 펀치 패턴 후 다른 패턴으로 전환)
        attackTime += Time.deltaTime;
        pattenTime += Time.deltaTime;
        if (attackTime >= 8.0f)
        {
            pattenOn = !pattenOn;
            attackTime = 0.0f;
        }
        if (pattenTime >= 7.0f)
        {
            pattenType = !pattenType;
            pattenTime = 0.0f;
        }
    }

    IEnumerator CheckGolemState()
    {
        while (!isDie)
        {
            // 0.1f 마다 상태 체크
            yield return new WaitForSeconds(0.1f);
            // 플레이어와의 거리 체크
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            // 땅찍기 패턴
            if (pattenOn == true && pattenType == true)
            {
                golemState = GolemState.groundAttack;
                yield return new WaitForSeconds(0.65f);
                summonExplosionEffect();
                pattenOn = false;
            }
            // 더블펀치 패턴
            if (pattenOn == true && pattenType == false)
            {
                golemState = GolemState.doublePunch;
                yield return new WaitForSeconds(0.3f);
                //for (int i = 0; i < 10; i++)
                //{
                //    yield return new WaitForSeconds(0.2f);
                //}
                pattenOn = false;
            }
            // 플레이어 공격
            if (dist <= attackDist && attackChecking.inAttackRange == true)
            {
                golemState = GolemState.attack;
            }
            // 플레이어 추적
            else if (dist <= traceDist)
            {
                Vector3 vec = playerTr.position - transform.position;
                vec.Normalize();
                Quaternion quaternion = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(vec), Time.deltaTime * 20);
                transform.rotation = quaternion;

                golemState = GolemState.run;
            }
            // idle 상태
            else if (dist >= traceDist)
            {
                golemState = GolemState.idle;
            }
        }
    }

    // 기본 액션 처리
    IEnumerator GolemAction()
    {
        while (!isDie)
        {
            switch (golemState)
            {
                //idle 상태
                case GolemState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsRun", false);
                    break;

                // 추적 상태
                case GolemState.run:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("IsRun", true);
                    animator.SetBool("IsPunch", false);
                    animator.SetBool("IsGroundAttack", false);
                    animator.SetBool("IsDoublePunch", false);
                    break;

                // 공격 상태
                case GolemState.attack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsPunch", true);
                    animator.SetBool("IsGroundAttack", false);
                    animator.SetBool("IsDoublePunch", false);
                    animator.SetBool("IsRun", false);
                    break;

                // 땅찍기 상태
                case GolemState.groundAttack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsGroundAttack", true);
                    animator.SetBool("IsDoublePunch", false);
                    animator.SetBool("IsRun", false);
                    break;

                // 더블펀치 상태
                case GolemState.doublePunch:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsDoublePunch", true);
                    animator.SetBool("IsGroundAttack", false);
                    animator.SetBool("IsRun", false);
                    break;
            }
            yield return null;
        }
    }

    // 몬스터가 플레이어의 공격을 받을 경우 처리
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Melee")
        {
            hp -= coll.gameObject.GetComponent<Weapon>().m_Damage;

            if (hp <= 0)
            {
                MonsterDie();
            }
            else
            {
                animator.SetTrigger("IsHit");
            }
        }
    }

    // 플레이어 사망 시
    void OnPlayerDie()
    {
        // 몬스터의 상태를 체크하는 코루틴 함수를 모두 정지
        StopAllCoroutines();
        // 추적을 정지하고 애니메이션 수행
        nvAgent.isStopped = true;
        animator.SetTrigger("IsPlayerDie");
    }

    // 몬스터 사망 시
    public void MonsterDie()
    {
        // 몬스터의 상태를 체크하는 코루틴 함수를 모두 정지
        StopAllCoroutines();
        isDie = true;
        golemState = GolemState.die;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        // 몬스터에 추가된 Collider를 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
    }
    void summonExplosionEffect()
    {
        for (int i = 0; i < 12; i++)
        {
            Instantiate(explosionObject, explosionPos[i].position, explosionPos[i].rotation);
        }
    }
}
