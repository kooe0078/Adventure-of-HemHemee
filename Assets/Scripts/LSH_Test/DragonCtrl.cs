using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DragonCtrl : BossCtrl
{
    // 몬스터의 상태 설정
    public enum DragonState { idle, run, attack, breath, cast, die };
    public DragonState dragonState = DragonState.idle;

    // 추적 범위, 공격 거리, 사망 여부 확인, 몬스터 체력
    public float traceDist = 50.0f;
    public float attackDist = 7.0f;
    public bool isDie = false;

    // 몬스터가 공격하는 플래그, 타이머
    bool pattenOn = false;
    bool pattenType = true; // true일때 브레스, false일때 스펠 캐스트
    public float attackTime = 0.0f;
    public float pattenTime = 0.0f;

    // 몬스터의 투사체 생성, 생성 위치
    public GameObject FireBall;
    public Transform breathFirepos;
    public Transform castFirepos;

    // 어택박스 체크 코드 호출
    AttackCheck attackChecking;

    // 몬스터 효과음 선언
    public AudioClip breathSound;
    public AudioClip biteSound;
    public AudioClip castSound;
    public AudioClip deathSound;
    AudioSource audioSource;

    // 다음 스테이지로 진행할 문 오브젝트
    public GameObject Door;

    void Start()
    {

        hp = maxHp;
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        

        StartCoroutine(this.CheckDragonState());
        StartCoroutine(this.DragonAction());

        // 플레이어가 몬스터의 앞에 있는지 체크하는 변수값 가져오기
        attackChecking = GameObject.Find("AttackPoint").GetComponent<AttackCheck>();
    }

    void Update()
    {
        // 드래곤의 공격 패턴 조정 (8초동안 물기패턴 후 다른 패턴으로 전환)
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
        Debug.Log(hp);
    }

    IEnumerator CheckDragonState()
    {
        while (!isDie)
        {
            // 0.1f 마다 상태 체크
            yield return new WaitForSeconds(0.1f);
            // 플레이어와의 거리 체크
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            // 브레스 패턴
            if (pattenOn == true && pattenType == true)
            {
                dragonState = DragonState.breath;
                audioSource.PlayOneShot(breathSound);
                yield return new WaitForSeconds(0.3f);
                Fire();
                yield return new WaitForSeconds(1.0f);
                pattenOn = false;
            }
            // 캐스트 패턴
            if (pattenOn == true && pattenType == false)
            {
                dragonState = DragonState.cast;
                audioSource.PlayOneShot(castSound);
                for (int i = 0; i < 10; i++)
                {
                    Cast();
                    yield return new WaitForSeconds(0.2f);
                }
                pattenOn = false;
            }
            // 플레이어 공격
            if (dist <= attackDist && attackChecking.inAttackRange == true)
            {
                dragonState = DragonState.attack;
                //audioSource.PlayOneShot(biteSound);
            }
            // 플레이어 추적
            else if (dist <= traceDist)
            {
                //Vector3 vec = playerTr.position - transform.position;
                //vec.Normalize();
                //Quaternion quaternion = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(vec), Time.deltaTime * 20);
                //transform.rotation = quaternion;

                dragonState = DragonState.run;
            }
            // idle 상태
            else if (dist >= traceDist)
            {
                dragonState = DragonState.idle;
            }
        }
    }

    // 기본 액션 처리
    IEnumerator DragonAction()
    {
        while (!isDie)
        {
            switch (dragonState)
            {
                //idle 상태
                case DragonState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsRun", false);
                    break;

                // 추적 상태
                case DragonState.run:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("IsBite", false);
                    animator.SetBool("IsBreath", false);
                    animator.SetBool("IsCast", false);
                    animator.SetBool("IsRun", true);
                    break;

                // 공격 상태
                case DragonState.attack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsBreath", false);
                    animator.SetBool("IsCast", false);
                    animator.SetBool("IsRun", false);
                    animator.SetBool("IsBite", true);
                    break;

                // 브레스 상태
                case DragonState.breath:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsBreath", true);
                    animator.SetBool("IsCast", false);
                    animator.SetBool("IsRun", false);
                    break;

                // 스펠 캐스트 상태
                case DragonState.cast:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsCast", true);
                    animator.SetBool("IsBreath", false);
                    animator.SetBool("IsRun", false);
                    break;
            }
            yield return null;
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
        dragonState = DragonState.die;
        audioSource.PlayOneShot(deathSound);
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        // 몬스터에 추가된 Collider를 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

        Door.gameObject.SetActive(true);
    }

    void Fire()
    {
        // 브레스 화염구 생성
        Instantiate(FireBall, breathFirepos.position, breathFirepos.rotation);
    }

    void Cast()
    {
        // 캐스트 화염구 생성
        Instantiate(FireBall, castFirepos.position, castFirepos.rotation);
    }
}

