using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepFleeBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [SerializeField]
    private float agentSpeedModifier = 1.5f;
    private float previousSpeed = 1f;


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
        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;
        if (closeRadius == null) closeRadius = sensors.closeCollider;

        previousSpeed = sheepAgent.speed;
        sheepAgent.speed = agentSpeedModifier * previousSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Time.timeScale != 0)
        {
            // detect enemy
            List<GameObject> wolves = visionRadius.GetAllColliders("Player");
            if (wolves.Count > 0)
            {
                Vector3 meanPosition = Vector3.zero;

                foreach (GameObject go in wolves)
                {
                    meanPosition += go.transform.position;
                }

                meanPosition /= wolves.Count;
                Vector3 oppositePosition = sheepTransform.position + (sheepTransform.position - meanPosition);

                if (sheepAgent.isActiveAndEnabled)
                    sheepAgent.SetDestination(oppositePosition);

                Debug.DrawLine(sheepTransform.position, oppositePosition, Color.magenta);
            }
            else
            {
                animator.SetBool("isFleeing", false);
                animator.SetBool("isIdling", true);
            }
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheepAgent.speed = previousSpeed;
    }
}
