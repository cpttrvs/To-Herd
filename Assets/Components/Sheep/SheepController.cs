using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepController : MonoBehaviour
{
    [SerializeField]
    private SheepSelector sheepSelector = null;
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    [SerializeField]
    private Animator animator;

    [Header("Agent")]
    [SerializeField]
    private Animator agent;
    [SerializeField]
    private float range;
    [SerializeField]
    private float followRadius;


    private bool hasAMoveOrder = false;
    private Vector3 currentMovePosition = Vector3.zero;

    private void Start()
    {
        sheepSelector.OnSelection += SheepSelector_OnSelection;
        sheepSelector.OnDeselection += SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder += SheepSelector_OnMoveOrder;

        animator.SetBool("isMoving", false);
        agent.SetBool("isIdling", true);
    }

    private void OnDestroy()
    {
        sheepSelector.OnSelection -= SheepSelector_OnSelection;
        sheepSelector.OnDeselection -= SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder -= SheepSelector_OnMoveOrder;
    }

    private void Update()
    {
        if(hasAMoveOrder)
        {
            animator.SetBool("isMoving", true);

            navMeshAgent.SetDestination(currentMovePosition);

            if(transform.position.x == currentMovePosition.x && transform.position.z == currentMovePosition.z)
            {
                hasAMoveOrder = false;
                currentMovePosition = Vector3.zero;
                animator.SetBool("isMoving", false);
                agent.SetBool("isFollowingOrder", false);
            }
        }
    }



    void SheepSelector_OnSelection()
    {
        Debug.Log("[Sheep] OnSelection: " + name);
    }

    void SheepSelector_OnDeselection()
    {
        Debug.Log("[Sheep] OnDeselection: " + name);
    }

    void SheepSelector_OnMoveOrder(Vector3 pos)
    {
        Debug.Log("[Sheep] OnMove: " + pos + ", " + name);

        hasAMoveOrder = true;
        currentMovePosition = pos;

        agent.SetBool("isFollowingOrder", true);

    }
}
