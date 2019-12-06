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
    private Animator animator = null;
    [SerializeField]
    private Animator agent = null;
    
    private Vector3 lastPosition = Vector3.zero;

    private void Start()
    {
        sheepSelector.OnSelection += SheepSelector_OnSelection;
        sheepSelector.OnDeselection += SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder += SheepSelector_OnMoveOrder;

        animator.SetBool("isMoving", false);
        agent.SetBool("isIdling", true);
        lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        sheepSelector.OnSelection -= SheepSelector_OnSelection;
        sheepSelector.OnDeselection -= SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder -= SheepSelector_OnMoveOrder;
    }

    private void Update()
    {
        if(lastPosition != transform.position)
        {
            animator.SetBool("isMoving", true);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        lastPosition = transform.position;
    }



    void SheepSelector_OnSelection(SheepSelector s)
    {
        Debug.Log("[Sheep] OnSelection: " + name);
    }

    void SheepSelector_OnDeselection(SheepSelector s)
    {
        Debug.Log("[Sheep] OnDeselection: " + name);
    }

    void SheepSelector_OnMoveOrder(Vector3 pos)
    {
        Debug.Log("[Sheep] OnMove: " + pos + ", " + name);
        
        navMeshAgent.SetDestination(pos);
        Debug.DrawLine(transform.position, pos, Color.red);

        agent.SetBool("isFollowingOrder", true);
        agent.SetBool("isIdling", false);
        agent.SetBool("isFollowing", false);
        agent.SetBool("isLookingOut", false);
    }

    public void LookOut()
    {
        Debug.Log("[Sheep] LookOut " + name);

        agent.SetBool("isLookingOut", true);
        agent.SetBool("isIdling", false);
        agent.SetBool("isFollowingOrder", false);
        agent.SetBool("isFollowing", false);
    }

    public void Follow()
    {
        Debug.Log("[Sheep] Follow " + name);

        agent.SetBool("isFollowing", true);
        agent.SetBool("isLookingOut", false);
        agent.SetBool("isIdling", false);
        agent.SetBool("isFollowingOrder", false);
    }
}
