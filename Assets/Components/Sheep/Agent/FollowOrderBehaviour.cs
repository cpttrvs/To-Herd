using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FollowOrderBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider followRadius = null;
    private CustomCollider wanderRadius = null;

    [SerializeField]
    private int frequencyCheck = 100;
    private int currentTimer = 0;
    private Vector3 previousPos = Vector3.zero;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[FollowOrderBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if (sensors == null)
            {
                Debug.LogError("[FollowOrderBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
            {
                Debug.LogError("[FollowOrderBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;
        if (followRadius == null) followRadius = sensors.followCollider;
        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;

        previousPos = sheepTransform.position;
        currentTimer = 0;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(sheepAgent.destination == sheepTransform.position)
        {
            animator.SetBool("isFollowingOrder", false);
            animator.SetBool("isIdling", true);
        } else
        {
            currentTimer++;
            if (currentTimer == frequencyCheck)
            {
                if (sheepTransform.position == previousPos)
                {
                    Debug.Log("[FollowOrderBehaviour] Same position, sanity check");
                    animator.SetBool("isFollowingOrder", false);
                    animator.SetBool("isIdling", true);
                }

                currentTimer = 0;
            }
        }

        previousPos = sheepTransform.position;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheepAgent.ResetPath();
    }
}
