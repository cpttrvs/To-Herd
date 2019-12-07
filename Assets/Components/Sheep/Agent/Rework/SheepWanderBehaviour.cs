using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepWanderBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [Header("Realistic wander")]
    [SerializeField]
    private float turnRate = 15;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 30;
    private int currentStep = 0;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[SheepWanderBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
            {
                Debug.LogError("[SheepWanderBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if (sensors == null)
            {
                Debug.LogError("[SheepWanderBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;

        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;

        if (closeRadius == null) closeRadius = sensors.closeCollider;

        currentStep = Random.Range(0, frequency);

        if (sheepAgent.isActiveAndEnabled)
            sheepAgent.ResetPath();
        //sheepAgent.isStopped = true;

        //Debug.Log(sheepTransform.name + " entered IDLE");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // realistic idle
        currentStep++;
        if (currentStep == frequency)
        {
            currentStep = 0;

            float angle = Random.Range(-turnRate, turnRate);

            sheepTransform.Rotate(Vector3.up, angle);

            if (sheepAgent.isActiveAndEnabled)
                sheepAgent.SetDestination(sheepTransform.position + sheepTransform.forward * movementDistance);

            Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.magenta);
        }

        // if a wolf in vision, flee
        List<GameObject> wolfInVision = visionRadius.GetAllColliders("Player");

        if (wolfInVision.Count > 0)
        {
            animator.SetBool("isWandering", false);
            animator.SetBool("isFleeing", true);
        }
        else
        {
            // if a sheep in medium, follow

            List<GameObject> sheepsInMedium = mediumRadius.GetAllColliders("Sheep");

            if (sheepsInMedium.Count > 1)
            {
                animator.SetBool("isWandering", false);
                animator.SetBool("isFollowing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //sheepAgent.isStopped = false;

    }
}
