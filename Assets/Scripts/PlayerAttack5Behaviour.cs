using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack5Behaviour : StateMachineBehaviour
{
    PlayerCtrl m_PlayerCtrl;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_PlayerCtrl = FindObjectOfType<PlayerCtrl>();
        animator.SetBool("Attack5", false);
        if (m_PlayerCtrl.noOfClicks >= 6)
        {
            animator.SetBool("AttacktoStand", true);
        }
    }
}