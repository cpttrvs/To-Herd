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
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [SerializeField]
    private float decisionTimer = 60;
    private float currentTimer = 0;

    [SerializeField]
    private int closeWeight = 1;
    [SerializeField]
    private int mediumWeight = 2;
    [SerializeField]
    private int visionWeight = 3;

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
        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;
        if (closeRadius == null) closeRadius = sensors.closeCollider;

        //Debug.Log(sheepTransform.name + " entered FOLLOW");
        currentTimer = decisionTimer;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(currentTimer % decisionTimer == 0)
        {
            List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

            if (visionObjects.Count > 1)
            {
                List<GameObject> mediumObjects = mediumRadius.GetAllColliders("Sheep");
                List<GameObject> closeObjects = closeRadius.GetAllColliders("Sheep");

                // mean position from every sheeps in vision
                // weighted position depending on radius

                Vector3 meanPos = Vector3.zero;
                int nbClose = 0;
                int nbMedium = 0;
                int nbVision = 0;
                foreach (GameObject go in visionObjects)
                {
                    if (go != sheepTransform.gameObject)
                    {
                        if(go.GetComponentInChildren<SheepController>().IsFollowingOrder())
                        {
                            Debug.Log("FOLLOWING " + go.name);
                            //prioritize the sheep following an order
                            meanPos += go.transform.position;
                            nbVision++;
                            break;
                        } else
                        {
                            if (closeObjects.Contains(go))
                            {
                                for (int i = 0; i < closeWeight; i++)
                                {
                                    meanPos += (go.transform.position);
                                    nbClose++;
                                }
                            }
                            else if (mediumObjects.Contains(go))
                            {
                                for (int i = 0; i < mediumWeight; i++)
                                {
                                    meanPos += (go.transform.position);
                                    nbMedium++;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < visionWeight; i++)
                                {
                                    meanPos += (go.transform.position);
                                    nbVision++;
                                }
                            }
                        }
                    }
                }

                int total = nbClose + nbMedium + nbVision;

                Debug.Log("follow: " + nbClose + " " + nbMedium + " " + nbVision);

                if (total > 0)
                {
                    meanPos /= (nbClose + nbMedium + nbVision);

                    if (sheepAgent.isActiveAndEnabled)
                        sheepAgent.SetDestination(meanPos);

                    Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.green);

                    currentTimer = 0;
                }
            }

        }
        currentTimer++;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
