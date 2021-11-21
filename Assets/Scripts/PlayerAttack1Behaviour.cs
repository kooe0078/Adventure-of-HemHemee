using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1Behaviour : StateMachineBehaviour
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
        Instantiate(m_PlayerCtrl.m_WeaponEffect, m_PlayerCtrl.m_AttackPos1.position, m_PlayerCtrl.m_AttackPos1.rotation);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PlayerCtrl = FindObjectOfType<PlayerCtrl>();
        m_PlayerCtrl.m_Animator.speed = 1;
        m_PlayerCtrl.canAttack = 0f;
        m_PlayerCtrl._isAttack = false;
        animator.SetBool("Attack1", false);
        if (m_PlayerCtrl.noOfClicks >= 2)
        {
            animator.SetBool("Attack2", true);
        }
    }
}