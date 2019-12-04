using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider followRadius = null;

    [Header("Follow state")]
    [SerializeField]
    private int numberOfSheepsRequired = 2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if(sensors == null)
            {
                Debug.LogError("[FollowBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
            {
                Debug.LogError("[FollowBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;
        if (followRadius == null) followRadius = sensors.followCollider;


        Debug.Log(sheepTransform.name + " entered FOLLOW");
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // move towards the closest sheep in vision
        Collider[] visionColliders = Physics.OverlapSphere(visionRadius.GetPosition(), visionRadius.GetRadius());

        float closestDistance = float.MaxValue;
        Vector3 closestSheep = Vector3.zero;

        for (int i = 0; i < visionColliders.Length; i++)
        {
            if(visionColliders[i].CompareTag("Sheep") && visionColliders[i].gameObject != sheepTransform.gameObject)
            {
                float distance = Vector3.Distance(sheepTransform.position, visionColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSheep = visionColliders[i].transform.position;
                }
            }
        }

        if(visionColliders.Length > 1)
        {
            sheepAgent.SetDestination(closestSheep);
        }

        // if enough sheeps in follow, back to idle
        Collider[] followColliders = Physics.OverlapSphere(followRadius.GetPosition(), followRadius.GetRadius());
        int numberOfSheepsInRadius = 0;
        for (int i = 0; i < followColliders.Length; i++)
        {
            if (followColliders[i].CompareTag("Sheep") && followColliders[i].gameObject != sheepTransform.gameObject)
                numberOfSheepsInRadius++;
        }

        if(numberOfSheepsInRadius >= numberOfSheepsRequired)
        {
            animator.SetBool("isFollowing", false);
            animator.SetBool("isIdling", true);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
