using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehaviour : StateMachineBehaviour
{
    private CustomCollider radius = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (radius == null)
        {
            radius = animator.GetComponentInChildren<SensorsLinker>().visionCollider;

            if (radius == null)
            {
                Debug.LogError("[FleeBehaviour] OnStateEnter: no collider found");
            }
        }

        radius.OnCollisionEntered += Radius_OnCollisionEntered;
        radius.OnCollisionExited += Radius_OnCollisionExited;
        radius.OnCollisionStayed += Radius_OnCollisionStayed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        radius.OnCollisionEntered -= Radius_OnCollisionEntered;
        radius.OnCollisionExited -= Radius_OnCollisionExited;
        radius.OnCollisionStayed -= Radius_OnCollisionStayed;
    }

    void Radius_OnCollisionEntered(Collision collision)
    {

    }

    void Radius_OnCollisionExited(Collision collision)
    {

    }
    void Radius_OnCollisionStayed(Collision collision)
    {

    }
}
