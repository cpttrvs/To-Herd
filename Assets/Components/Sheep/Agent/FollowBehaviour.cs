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
        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;


        Debug.Log(sheepTransform.name + " entered FOLLOW");
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // move at the mean position of all sheeps in visionRadius
        List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

        Vector3 meanPosition = sheepTransform.position;
        foreach(GameObject go in visionObjects)
        {
            if(go != sheepTransform.gameObject)
            {
                meanPosition += go.transform.position;
            }
        }
        meanPosition /= visionObjects.Count;

        sheepAgent.SetDestination(meanPosition);
        Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.green);


        // if at least one is in wanderRadius, go to idle
        List<GameObject> wanderObjects = wanderRadius.GetAllColliders("Sheep");
        if(wanderObjects.Count > numberOfSheepsRequired)
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
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
