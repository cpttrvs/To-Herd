﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepIdleBehaviour : StateMachineBehaviour
{
    private SensorsLinker sensors = null;
    private Transform sheepTransform = null;
    private NavMeshAgent sheepAgent = null;

    private CustomCollider visionRadius = null;
    private CustomCollider mediumRadius = null;
    private CustomCollider closeRadius = null;

    [SerializeField]
    private int numberOfSheepsToFlock = 2;

    [Header("Realistic idle")]
    [SerializeField]
    private float turnRate = 30;
    [SerializeField]
    private float movementDistance = 2;
    [SerializeField]
    private int frequency = 100;
    private int currentStep = 0;


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

        if (sheepTransform == null)
        {
            sheepTransform = animator.gameObject.transform;

            if (sheepTransform == null)
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

        currentStep = Random.Range(0, frequency);

        if (sheepAgent.isActiveAndEnabled)
            sheepAgent.ResetPath();
        //sheepAgent.isStopped = true;

        //Debug.Log(sheepTransform.name + " entered IDLE");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // realistic idle
        currentStep++;
        if (currentStep == frequency)
        {
            currentStep = 0;

            float angle = Random.Range(-turnRate, turnRate);

            sheepTransform.Rotate(Vector3.up, angle);
            

            if (sheepAgent.isActiveAndEnabled)
                sheepAgent.SetDestination(sheepTransform.position + sheepTransform.forward * movementDistance);

            Debug.DrawLine(sheepTransform.position, sheepAgent.destination, Color.blue);
        }

        // if a wolf in vision, flee
        List<GameObject> wolfInVision = visionRadius.GetAllColliders("Player");

        if(wolfInVision.Count > 0)
        {
            animator.SetBool("isIdling", false);
            animator.SetBool("isFleeing", true);
        } else
        {
            // if only itself in medium, wander
            List<GameObject> sheepsInMedium = mediumRadius.GetAllColliders("Sheep");

            if (sheepsInMedium.Count == 1)
            {
                animator.SetBool("isIdling", false);
                animator.SetBool("isWandering", true);
            } else
            {
                // if less than nbSheepsToFlock in close, follow

                List<GameObject> sheepsInClose = closeRadius.GetAllColliders("Sheep");

                if (sheepsInClose.Count <= numberOfSheepsToFlock && sheepsInClose.Count > 1)
                {
                    animator.SetBool("isIdling", false);
                    animator.SetBool("isFollowing", true);
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //sheepAgent.isStopped = false;

    }
}
