using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookOutBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sheepAgent == null)
        {
            sheepAgent = animator.GetComponentInChildren<NavMeshAgent>();

            if (sheepAgent == null)
            {
                Debug.LogError("[LookOutBehaviour] OnStateEnter: no nav mesh agent found");
            }
        }

        if (sensors == null)
        {
            sensors = animator.GetComponentInChildren<SensorsLinker>();

            if (sensors == null)
            {
                Debug.LogError("[LookOutBehaviour] OnStateEnter: no sensors linker found");
            }
        }

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
            {
                Debug.LogError("[LookOutBehaviour] OnStateEnter: no sheep transform found");
            }
        }

        if (visionRadius == null) visionRadius = sensors.visionCollider;
        if (mediumRadius == null) mediumRadius = sensors.mediumCollider;
        if (closeRadius == null) closeRadius = sensors.closeCollider;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //immobilised, will face mean pos of enemies in range

        List<GameObject> visionObjects = visionRadius.GetAllColliders("Enemy");

        if(visionObjects.Count > 0)
        {
            Vector3 meanPos = Vector3.zero;
            foreach (GameObject go in visionObjects)
            {
                meanPos += go.transform.position;
            }
            meanPos /= visionObjects.Count;

            sheepTransform.LookAt(meanPos); //TODO: lerp

            Debug.DrawLine(sheepTransform.position, meanPos, Color.magenta);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
