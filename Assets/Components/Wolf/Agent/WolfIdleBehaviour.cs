using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfIdleBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform wolfTransform = null;
    private NavMeshAgent wolfAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [Header("Realistic idle")]
    [SerializeField]
    private float turnRate = 30;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 100;
    private int currentStep = 0;

    [Header("Flee")]
    [SerializeField]
    private int numberOfSheepsToFlee = 3;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wolfAgent == null)
        {
            wolfAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (wolfAgent == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (wolfTransform == null)
        {
            wolfTransform = animator.gameObject.transform;

            if (wolfTransform == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if (sensors == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;

        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;

        if (closeRadius == null) closeRadius = sensors.closeCollider;

        currentStep = Random.Range(0, frequency);

        wolfAgent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // realistic idle
        currentStep++;
        if (currentStep == frequency)
        {
            currentStep = 0;

            float angle = Random.Range(-turnRate, turnRate);

            wolfTransform.Rotate(Vector3.up, angle);

            wolfAgent.SetDestination(wolfTransform.position + wolfTransform.forward * movementDistance);

            Debug.DrawLine(wolfTransform.position, wolfAgent.destination, Color.blue);
        }

        List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

        if(visionObjects.Count > 0)
        {
            if(visionObjects.Count >= numberOfSheepsToFlee)
            {
                animator.SetBool("isIdling", false);
                animator.SetBool("isFleeing", true);
            } else
            {
                animator.SetBool("isIdling", false);
                animator.SetBool("isChasing", true);
            }
        }
    }
}
