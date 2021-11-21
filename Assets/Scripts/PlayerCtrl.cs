using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Transactions;
using System.Xml.Schema;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum e_CharacterState
{
    IDLE,
    RUN,
    ROLLING,
    JUMP,
    ATTACK,
}
public class PlayerCtrl : MonoBehaviour
{

    // 플레이어 게임 오버 상태 유무
    public bool _gameOver;
    // 플레이어의 이동 스피드
    public float m_Speed;
    // 플레이어의 최대 이동속도
    private const float const_MaxSpeed = 5.0f;
    public float m_MaxStamina = 100.0f;
    public float m_currentStamina;

    public float maxHP; // 플레이어의 최대HP
    public float HP; // 플레이어의 HP



    public LayerMask m_GroundLayers;
    public CapsuleCollider m_Collider;

    private const float const_Dash_Cooltime = 5.0f;
    public float m_currentCooltime;

    public float m_MaxActive_Cooltime = 5.0f;
    public float m_currentActiveCooltime;

    public float m_DashSpeed;
    // private float m_Dashwait;

    // 플레이어의 점프력
    private float m_JumpForce = 5.0f;
    private float m_AnimHeight;
    public float m_TurnSpeed;
    public float m_TurnVelocity;
    private e_CharacterState m_CharacterState;

    // 플레이어를 따라다닐 카메라 오브젝트
    private new GameObject camera;
    public GameObject Camera
    {
        get
        {
            if (camera == null)
                camera = GameObject.FindGameObjectWithTag("PlayerCamera");
            return camera;
        }
        set { camera = value; }
    }

    // 플레이어 캐릭터 컨트롤러와 애니메이터
    public Animator m_Animator;

    private Rigidbody m_Rigidbody;

    // 전투 관련 변수
    public float canAttack;
    int m_ComboCount;
    public Transform m_AttackPos1;
    public Transform m_AttackPos2;
    public Transform m_AttackPos3;
    public Transform m_AttackPos4;
    public GameObject m_WeaponEffect;
    public GameObject m_WeaponEffect2;
    public GameObject m_WeaponEffect3;
    public GameObject m_WeaponEffect4;
    public GameObject m_ActiveItem;


    // 플레이어 상태 체크 변수
    bool _isDodge;
    bool _isJump;
    public bool _isAttack;

    // 공격 관련 변수
    public Weapon m_EquipWeapon;
    public GameObject WeaponPos;
    public Transform FirePos;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    const float maxComboDealy = 0.5f;


    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();
        transform.name = "Player";

        camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        m_currentStamina = m_MaxStamina;
        HP = maxHP;
        m_ComboCount = 0;
    }

    private void Update()
    {

        // 공격 모션
        // 플레이어가 게임 오버 상태인지 아닌지를 판단
        if (_gameOver == false)
        {
            if (Time.time - lastClickedTime > maxComboDealy)
            {
                noOfClicks = 0;
            }
            Swap();
            m_currentStamina = Refillresource(m_currentStamina, m_MaxStamina, 20);
            m_currentActiveCooltime = Refillresource(m_currentActiveCooltime, m_MaxActive_Cooltime, 1);
            if (Input.GetMouseButtonDown(0)) { OnStateAttack(); }
            if (Input.GetKeyDown(KeyCode.Q)) { UseActiveItem(); }
            m_AnimHeight = Mathf.Clamp(m_Rigidbody.velocity.y, -3, 3);

            if (!_isAttack)
            {
                Movement();
            }
            bool ground = _IsGrounded(); // ground 안에 _IsGrounded()의 값이 존재함
            m_Animator.SetBool("isGround", ground);
            m_Animator.SetFloat("Height", m_AnimHeight); // 높이가 바뀔 때에 값이 갱신되어야 함.
            m_ComboCount = 0;
        }
    }
    void UseActiveItem()
    {
        if(m_currentActiveCooltime >= 5)
        {
            m_currentActiveCooltime = 0;
            Instantiate(m_ActiveItem, FirePos.position, FirePos.rotation);
        }
        
    }
    void Swap()
    {
        if (m_EquipWeapon != null)
        {
            m_EquipWeapon.gameObject.SetActive(true);
        }
    }
    void ComboAttack()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            m_Animator.SetBool("Attack1", true);
        }

        noOfClicks = Mathf.Clamp(noOfClicks, 0, 5);
    }
    void OnstateChange(e_CharacterState newState)
    {
        switch (newState)
        {
            case e_CharacterState.IDLE:
                OnStateIdle();
                break;
            case e_CharacterState.RUN:
                OnStateRun();
                break;
            case e_CharacterState.ROLLING:
                OnStateRolling();
                break;
            case e_CharacterState.JUMP:
                OnStateJump();
                break;
            case e_CharacterState.ATTACK:
                OnStateAttack();
                break;
        }
    }

    public void OnStateIdle()
    {
    }
    public void OnStateRun()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontal, 0, vertical);
        if (_IsGrounded())
        {
            m_Animator.SetFloat("Speed", inputVector.magnitude * 2);
        }

    }
    public void OnStateRolling()
    { }
    public void OnStateJump()
    {
        if (_IsGrounded())
        {
            _isJump = true;
            m_Animator.SetTrigger("Jump");
            Debug.Log(m_Animator.GetFloat("Height"));
            m_Animator.SetFloat("Height", m_AnimHeight);
        }
        _isJump = false;
    }
    public void OnStateAttack()
    {
        ComboAttack();
    }
    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontal, 0, vertical);
        inputVector.Normalize();
        OnstateChange(e_CharacterState.RUN);
        Vector3 cameraDir = Vector3.zero;
        if (Camera != null)
            cameraDir = Camera.transform.rotation * inputVector;
        Vector3 moveDir = new Vector3(cameraDir.x, 0f, cameraDir.z);
        moveDir.Normalize();
        if (moveDir.magnitude >= 0.1f)
        {
            float playerAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, playerAngle, ref m_TurnVelocity, m_TurnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            m_Rigidbody.AddForce(moveDir * m_Speed * 10);
            Dash(moveDir);
        }
        Jump();
    }

    float Refillresource(float current, float Max, float figure)
    {
        // 플레이어가 사용한 자원을 current 수치까지 Time.deltaTime * figure 속도만큼 복구하는 함수
        if (current <= Max)
            current += Time.deltaTime * figure;
        return current;
    }
    void Dash(Vector3 RollingDir)
    {
        if (m_currentStamina >= 100 && Input.GetKey(KeyCode.LeftShift))
        {
            OnstateChange(e_CharacterState.ROLLING);
            m_Animator.SetTrigger("IsRoll");
            m_currentStamina -= 100;
            m_currentCooltime = 0;
            //  m_Dashwait = 0;
        }
    }
    private bool _IsGrounded()
    {
        bool road = Physics.CheckCapsule(m_Collider.bounds.center, new Vector3(m_Collider.bounds.center.x, m_Collider.bounds.min.y, m_Collider.bounds.center.z), m_Collider.radius * 1.0f, m_GroundLayers);
        _isJump = road;
        return road;
    }
    void Jump()
    {
        if (_IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            OnstateChange(e_CharacterState.JUMP);
            m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
        }
    }
    public void SetStat(int hp, int damage, double attackspeed)
    {
        maxHP = hp;
        m_EquipWeapon.m_Damage = damage;
        m_EquipWeapon.m_AttackSpeed = (float)attackspeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MonsterAttack")
        {
            if (_gameOver == false)
            {
                HP -= 5;
                if (HP <= 0)
                {
                    _gameOver = true;
                    m_Animator.SetTrigger("isDie");
                    m_Animator.SetBool("Die", true);
                }
            }
        }

        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.HP:
                    HP += item.value;
                    if (HP > maxHP)
                        HP = maxHP;
                    break;
                case Item.Type.Weapon:
                    maxHP += item.HpVaule;
                    m_EquipWeapon.m_Damage += item.DamageValue;
                    m_EquipWeapon.m_AttackSpeed += item.AttackSpeedValue;
                    m_EquipWeapon.Effects = Instantiate(item.WeaponEffects, this.gameObject.transform);
                    break;
                case Item.Type.Stat:
                    m_Speed += item.StatValue;
                    m_EquipWeapon.Effects = Instantiate(item.WeaponEffects, this.gameObject.transform);
                    break;
                case Item.Type.Accessory:
                    m_ActiveItem = other.GetComponent<Item>().WeaponEffects;
                    break;

            }
            Destroy(other.gameObject);
        }
    }
    public void GameStart()
    {
        _gameOver = false;
    }
}
