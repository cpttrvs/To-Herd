using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfChaseBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform wolfTransform = null;
    private NavMeshAgent wolfAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;
    
    [Header("Flee")]
    [SerializeField]
    private int numberOfSheepsToFlee = 3;
    [SerializeField]
    private float angle = 10f;

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
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if more than the number in medium radius, flee, else chase the closest to kill
        List<GameObject> visionObjects = visionRadius.GetAllColliders("Sheep");

        if (visionObjects.Count > 0)
        {
            GameObject closestSheep = null;
            float distance = float.MaxValue;
            float temp = 0;

            foreach(GameObject go in visionObjects)
            {
                temp = Vector3.Distance(wolfTransform.position, go.transform.position);
                if(temp < distance)
                {
                    distance = temp;
                    closestSheep = go;
                }
            }

            if(closestSheep != null)
            {
                wolfAgent.SetDestination(closestSheep.transform.position);
                Debug.DrawRay(wolfTransform.position, wolfAgent.destination, Color.red);

                if(closestSheep.transform.position == wolfTransform.position)
                {
                    //considered dead
                    animator.SetBool("isChasing", false);
                    animator.SetBool("isIdling", true);
                }
            }

            // if the number of sheeps is facing it, flee
            int nbOfSheepsFacing = 0;
            foreach(GameObject go in visionObjects)
            {
                if(Vector3.Angle(go.transform.forward, wolfTransform.position - go.transform.position) < angle)
                {
                    nbOfSheepsFacing++;
                }
            }

            if(nbOfSheepsFacing >= numberOfSheepsToFlee)
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isFleeing", true);
            }
        } else
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isIdling", true);
        }
    }
}
