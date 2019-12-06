using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfFleeBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform wolfTransform = null;
    private NavMeshAgent wolfAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wolfAgent == null)
        {
            wolfAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (wolfAgent == null)
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

        if (wolfTransform == null)
        {
            wolfTransform = animator.gameObject.transform;

            if (wolfTransform == null)
            {
                Debug.LogError("[FleeBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;
        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;
        if (closeRadius == null) closeRadius = sensors.closeCollider;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // detect enemy
        List<GameObject> enemies = visionRadius.GetAllColliders("Sheep");
        if (enemies.Count > 0)
        {
            Vector3 meanPosition = Vector3.zero;

            foreach (GameObject go in enemies)
            {
                meanPosition += go.transform.position;
            }

            meanPosition /= enemies.Count;
            Vector3 oppositePosition = wolfTransform.position + (wolfTransform.position - meanPosition);

            wolfAgent.SetDestination(oppositePosition);

            Debug.DrawLine(wolfTransform.position, oppositePosition, Color.magenta);
        }
        else
        {
            animator.SetBool("isFleeing", false);
            animator.SetBool("isIdling", true);
        }
    }
}
