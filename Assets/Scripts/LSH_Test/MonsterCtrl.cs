using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    // 몬스터의 상태 설정
    public enum MonsterState { idle, walk, run, attack, die };
    public MonsterState monsterState = MonsterState.idle;

    // 몬스터의 위치
    private Transform monsterTr;
    // 플레이어의 위치
    private Transform playerTr;
    // 네비게이션
    private NavMeshAgent nvAgent;
    // 애니메이션
    private Animator animator;

    // 추적 범위, 공격 거리, 사망 여부 확인, 몬스터 체력
    public float traceDist = 10.0f;
    public float attackDist = 4.0f;
    public bool isDie = false;
    public int hp = 100;

    // 몬스터의 랜덤 이동 플래그와 타이머
    bool setIdle = true;
    public float setIdleTime = 0.0f;

    // 몬스터의 공격 딜레이 플래그와 타이머
    bool attackFlag = true;
    public float setAttackTime = 0.0f;

    // 몬스터의 패트롤 좌표값 배열 선언, 배열의 랜덤 선언
    [SerializeField] Transform[] m_PatrolPoints = null;
    int mRandom = 0;

    // 어택박스 체크 코드 호출
    AttackCheck attackChecking;

    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());

        // 플레이어가 몬스터의 앞에 있는지 체크하는 변수값 가져오기
        attackChecking = GetComponentInChildren<AttackCheck>();
    }

    void Update()
    {
        // 몬스터를 5초마다 랜덤한 위치로 움직이게 하기 위한 타이머
        setIdleTime += Time.deltaTime;
        setAttackTime += Time.deltaTime;
        if (setIdleTime > 5.0f)
        {
            setIdle = !setIdle;
            setIdleTime = 0.0f;
            mRandom = Random.Range(0, 4);
        }
        // 몬스터 공격 딜레이가 있고, 딜레이 지속 시간이 지났을 경우 공격으로 전환
        if (!attackFlag && setAttackTime > 2.0f)
        {
            attackFlag = true;
            setAttackTime = 0.0f;
        }

    }

    // 몬스터의 상태 처리
    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 0.1f 마다 상태 체크
            yield return new WaitForSeconds(0.1f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);
            // 플레이어 공격
            if (dist <= traceDist)
            {
                Debug.Log("Dist : " + dist + ", Check : " + attackChecking.inAttackRange);
            }
            if (dist <= attackDist && attackChecking.inAttackRange == true)
            {
                Debug.Log("공격중");
                if (attackFlag)
                {
                    monsterState = MonsterState.attack;
                    attackFlag = false;
                    attackChecking.inAttackRange = false;
                }
            }
            // 플레이어 추적
            else if (dist <= traceDist)
            {
                Vector3 vec = playerTr.position - transform.position;
                vec.Normalize();
                Quaternion quaternion = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(vec), Time.deltaTime * 20);
                transform.rotation = quaternion;

                monsterState = MonsterState.run;
            }
            // idle, 보통 걸음 상태
            else if (dist >= traceDist)
            {
                if (setIdle == true)
                    monsterState = MonsterState.idle;
                else
                    monsterState = MonsterState.walk;
            }
        }
    }

    // 몬스터의 기본 액션 처리
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                //idle 상태
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsWalk", false);
                    break;

                // 보통 걸음 상태
                case MonsterState.walk:
                    nvAgent.isStopped = false;
                    // 몬스터의 패트롤
                    nvAgent.destination = m_PatrolPoints[mRandom].position;
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsRun", false);                    
                    break;

                // 추적 상태
                case MonsterState.run:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("IsRun", true);
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsAttack", false);
                    break;

                // 공격 상태
                case MonsterState.attack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsRun", false);
                    animator.SetBool("IsAttack", true);                    
                    break;
            }
            yield return null;
        }
    }

    // 몬스터가 무언가와 충돌했을 경우 처리
    private void OnCollisionEnter(Collision coll)
    {
        // 몬스터가 플레이어의 공격을 받을 경우 처리
        if (coll.gameObject.tag == "Melee")
        {
            Destroy(coll.gameObject);
            //hp -= coll.gameObject.GetComponent<PlayerCtrl>().damage;

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
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        // 몬스터에 추가된 Collider를 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
    }
}

