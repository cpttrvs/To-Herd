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
    private CustomCollider wanderRadius = null;

    [SerializeField]
    private float decisionTimer = 60;
    private float currentTimer = 0;

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
        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;

        //Debug.Log(sheepTransform.name + " entered FOLLOW");
        currentTimer = decisionTimer;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");
        List<GameObject> wanderObjects = wanderRadius.GetAllColliders("Sheep");

        if (currentTimer == decisionTimer)
        {
            // move to the farest sheep
            float distance = 0;
            Vector3 farestPos = sheepTransform.position;
            foreach (GameObject go in visionObjects)
            {
                if (go != sheepTransform.gameObject)
                {
                    float temp = Vector3.Distance(sheepTransform.position, go.transform.position);

                    if (temp > distance)
                    {
                        distance = temp;
                        farestPos = go.transform.position;
                    }
                }
            }

            sheepAgent.SetDestination(farestPos);
            Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.green);

            currentTimer = 0;
        }

        // if there are the same amount of sheep in wander than in vision, then go to idle
        if(wanderObjects.Count == visionObjects.Count)
        {
            animator.SetBool("isFollowing", false);
            animator.SetBool("isIdling", true);
        }

        // if no one in sight, go to wander
        if(visionObjects.Count == 1)
        {
            animator.SetBool("isFollowing", false);
            animator.SetBool("isWandering", true);
        }

        currentTimer++;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
