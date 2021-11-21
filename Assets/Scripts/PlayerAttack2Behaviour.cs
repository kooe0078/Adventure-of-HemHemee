using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2Behaviour : StateMachineBehaviour
{
    PlayerCtrl m_PlayerCtrl;
    public AudioSource source = null;
    public AudioClip clip;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PlayerCtrl = FindObjectOfType<PlayerCtrl>();
        m_PlayerCtrl.m_Animator.speed = m_PlayerCtrl.m_EquipWeapon.m_AttackSpeed;
        m_PlayerCtrl.canAttack = 1.0f;
        m_PlayerCtrl._isAttack = true;
        source = m_PlayerCtrl.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(clip);
        Instantiate(m_PlayerCtrl.m_WeaponEffect2, m_PlayerCtrl.m_AttackPos2.position, m_PlayerCtrl.m_AttackPos2.rotation);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PlayerCtrl = FindObjectOfType<PlayerCtrl>();
        m_PlayerCtrl.m_Animator.speed = 1;
        m_PlayerCtrl.canAttack = 0f;
        m_PlayerCtrl._isAttack = false;
        animator.SetBool("Attack2", false);
        if (m_PlayerCtrl.noOfClicks >= 3)
        {
            animator.SetBool("Attack3", true);
        }
    }
}