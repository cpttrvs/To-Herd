using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider followRadius = null;
    private CustomCollider wanderRadius = null;

    [Header("Realistic wander")]
    [SerializeField]
    private float turnRate = 20;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 50;
    private int currentStep = 0;

    [Header("Follow state")]
    [SerializeField]
    private int numberOfSheepsRequired = 1;

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

            if (sensors == null)
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
        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;

        currentStep = 0;

        //Debug.Log(sheepTransform.name + " entered WANDER");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // realistic wander
        currentStep++;
        if (currentStep == frequency)
        {
            currentStep = 0;

            float angle = Random.Range(-turnRate, turnRate);

            sheepTransform.Rotate(Vector3.up, angle);

            sheepAgent.SetDestination(sheepTransform.position + sheepTransform.forward * movementDistance);
        }

        // if at least one is in wander, go to idle
        List<GameObject> wanderObjects = wanderRadius.GetAllColliders("Sheep");

        if(wanderObjects.Count > 1)
        {
            animator.SetBool("isWandering", false);
            animator.SetBool("isIdling", true);
        } else
        {
            // if at least one is in vision radius, go to follow
            List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

            if (visionObjects.Count > 1)
            {
                animator.SetBool("isWandering", false);
                animator.SetBool("isFollowing", true);
            }
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
