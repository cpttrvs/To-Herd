using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider followRadius = null;
    private CustomCollider wanderRadius = null;

    [Header("Realistic idle")]
    [SerializeField]
    private float turnRate = 30;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 100;
    [SerializeField]
    private int variation = 50;
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

        if(sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if(sensors == null)
            {
                Debug.LogError("[IdleBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;

        if (followRadius == null) followRadius = sensors.followCollider;

        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;

        currentStep = Random.Range(0, frequency);

        sheepAgent.ResetPath();
        //sheepAgent.isStopped = true;

        //Debug.Log(sheepTransform.name + " entered IDLE");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // detect enemy
        List<GameObject> enemies = visionRadius.GetAllColliders("Enemy");
        if(enemies.Count > 0)
        {
            animator.SetBool("isIdling", false);
            animator.SetBool("isFleeing", true);
        } else
        {
            // realistic idle
            currentStep++;
            if (currentStep == frequency)
            {
                currentStep = 0;

                float angle = Random.Range(-turnRate, turnRate);

                sheepTransform.Rotate(Vector3.up, angle);

                sheepAgent.SetDestination(sheepTransform.position + sheepTransform.forward * movementDistance);

                Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.blue);
            }

            // if there are more sheeps in vision than in wander, go to follow
            List<GameObject> wanderObjects = wanderRadius.GetAllColliders("Sheep");
            List<GameObject> followObjects = followRadius.GetAllColliders("Sheep");
            List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

            if (visionObjects.Count > wanderObjects.Count)
            {
                animator.SetBool("isIdling", false);
                animator.SetBool("isFollowing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //sheepAgent.isStopped = false;

    }
}
