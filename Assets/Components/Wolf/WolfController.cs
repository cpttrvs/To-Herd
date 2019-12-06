using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private Animator agent = null;

    private Vector3 lastPosition = Vector3.zero;

    private void Start()
    {
        animator.SetBool("isIdling", true);
        agent.SetBool("isIdling", true);
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (lastPosition != transform.position)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        lastPosition = transform.position;
    }
}
