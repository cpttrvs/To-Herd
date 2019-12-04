using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBehaviour : StateMachineBehaviour
{
    private CustomCollider visionRadius = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    [Header("Realistic idle")]
    [SerializeField]
    private float turnRate = 30;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 100;
    private int currentStep = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if(sheepAgent == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if(sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if(sheepTransform == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null)
        {
            visionRadius = animator.GetComponentInChildren<SensorsLinker>().visionCollider;

            if (visionRadius == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no collider found");
            }
        }

        visionRadius.OnCollisionEntered += VisionRadius_OnCollisionEntered;
        visionRadius.OnCollisionExited += VisionRadius_OnCollisionExited;
        visionRadius.OnCollisionStayed += VisionRadius_OnCollisionStayed;

        currentStep = Random.Range(0, frequency);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentStep++;
        if(currentStep == frequency)
        {
            currentStep = 0;

            float angle = Random.Range(-turnRate, turnRate);

            sheepTransform.Rotate(Vector3.up, angle);

            sheepAgent.SetDestination(sheepTransform.position + sheepTransform.forward * movementDistance);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visionRadius.OnCollisionEntered -= VisionRadius_OnCollisionEntered;
        visionRadius.OnCollisionExited -= VisionRadius_OnCollisionExited;
        visionRadius.OnCollisionStayed -= VisionRadius_OnCollisionStayed;
    }

    void VisionRadius_OnCollisionEntered(Collision collision)
    {

    }

    void VisionRadius_OnCollisionExited(Collision collision)
    {

    }
    void VisionRadius_OnCollisionStayed(Collision collision)
    {

    }
}
