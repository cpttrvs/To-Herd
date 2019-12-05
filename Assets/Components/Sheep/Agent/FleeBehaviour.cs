using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider followRadius = null;
    private CustomCollider wanderRadius = null;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[FleeBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if (sensors == null)
            {
                Debug.LogError("[FleeBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
            {
                Debug.LogError("[FleeBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;
        if (followRadius == null) followRadius = sensors.followCollider;
        if (wanderRadius == null) wanderRadius = sensors.wanderCollider;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // detect enemy
        List<GameObject> enemies = visionRadius.GetAllColliders("Enemy");
        if(enemies.Count > 0)
        {
            Vector3 meanPosition = Vector3.zero;

            foreach(GameObject go in enemies)
            {
                meanPosition += go.transform.position;
            }

            meanPosition /= enemies.Count;
            Vector3 oppositePosition = sheepTransform.position + (sheepTransform.position - meanPosition);

            sheepAgent.SetDestination(oppositePosition);

            Debug.DrawLine(sheepTransform.position, oppositePosition, Color.magenta);
        } else
        {
            animator.SetBool("isFleeing", false);
            animator.SetBool("isIdling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
