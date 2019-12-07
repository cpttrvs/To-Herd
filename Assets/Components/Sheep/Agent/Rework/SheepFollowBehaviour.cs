using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepFollowBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [SerializeField]
    private float decisionTimer = 60;
    private float currentTimer = 0;

    [SerializeField]
    private int nbSheepsToIdle = 2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[FollowBehaviour] OnStateEnter: no nav mesh agent found");
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
        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;
        if (closeRadius == null) closeRadius = sensors.closeCollider;

        //Debug.Log(sheepTransform.name + " entered FOLLOW");
        currentTimer = decisionTimer;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentTimer % decisionTimer == 0)
        {
            List<GameObject> sheepsInVision = visionRadius.GetAllColliders("Sheep");

            if (sheepsInVision.Count > 1)
            {
                // mean position from every sheeps in vision
                // weighted position depending on radius

                Vector3 meanPos = Vector3.zero;

                foreach (GameObject go in sheepsInVision)
                {
                    meanPos += go.transform.position;
                }

                meanPos /= sheepsInVision.Count;

                if (sheepAgent.isActiveAndEnabled)
                    sheepAgent.SetDestination(meanPos);

                Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.green);

                currentTimer = 0;
                
            }

        }

        // if a wolf in vision, flee
        List<GameObject> wolfInVision = visionRadius.GetAllColliders("Player");

        if (wolfInVision.Count > 0)
        {
            animator.SetBool("isFollowing", false);
            animator.SetBool("isFleeing", true);
        }
        else
        {
            // if enough sheeps in close, go to idle
            List<GameObject> closeSheeps = closeRadius.GetAllColliders("Sheep");

            if (closeSheeps.Count > nbSheepsToIdle)
            {
                animator.SetBool("isFollowing", false);
                animator.SetBool("isIdling", true);
            }
            else
            {
                // if no sheep in medium, go to wander
                List<GameObject> mediumSheeps = mediumRadius.GetAllColliders("Sheep");

                if(mediumSheeps.Count == 1)
                {
                    animator.SetBool("isFollowing", false);
                    animator.SetBool("isWandering", true);
                }
            }
        }

        currentTimer++;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
