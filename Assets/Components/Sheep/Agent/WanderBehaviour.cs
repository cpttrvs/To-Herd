﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : StateMachineBehaviour
{
    private CustomCollider radius = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (radius == null)
        {
            radius = animator.GetComponentInChildren<SensorsLinker>().wanderCollider;

            if (radius == null)
            {
                Debug.LogError("[WanderBehaviour] OnStateEnter: no collider found");
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
