﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

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

    private bool isDead = false;

    private bool isMoving = false;

    private Vector3 lastPosition = Vector3.zero;

    public Action OnFollow;
    public Action OnStopFollow;
    public Action OnLookOut;
    public Action OnStopLookOut;

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
        if(!isDead)
        {
            if (lastPosition != transform.position)
            {
                animator.SetBool("isMoving", true);
                isMoving = true;
            }
            else
            {
                animator.SetBool("isMoving", false);
                isMoving = false;
            }

            lastPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            //dead
            isDead = true;

            agent.SetBool("isDead", true);
            animator.SetBool("isDead", true);
            gameObject.tag = "DeadSheep";

            navMeshAgent.enabled = false;
        }
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
        if(!isDead)
        {
            Debug.Log("[Sheep] OnMove: " + pos + ", " + name);

            StopLookOut();
            StopFollow();

            agent.SetBool("isIdling", false);
            agent.SetBool("isFollowing", false);
            agent.SetBool("isLookingOut", false);
            agent.SetBool("isFollowingOrder", true);

            navMeshAgent.SetDestination(pos);
            Debug.DrawLine(transform.position, pos, Color.red);
        }

    }

    public void LookOut()
    {
        if(!isDead)
        {
            Debug.Log("[Sheep] LookOut " + name);

            agent.SetBool("isLookingOut", true);
            agent.SetBool("isIdling", false);
            agent.SetBool("isFollowingOrder", false);
            agent.SetBool("isFollowing", false);

            OnLookOut?.Invoke();
        }
    }

    public void Follow()
    {
        if(!isDead)
        {
            Debug.Log("[Sheep] Follow " + name);

            agent.SetBool("isFollowing", true);
            agent.SetBool("isLookingOut", false);
            agent.SetBool("isIdling", false);
            agent.SetBool("isFollowingOrder", false);

            OnFollow?.Invoke();
        }
    }

    public void StopLookOut()
    {
        if (!isDead)
        {
            Debug.Log("[Sheep] Stop LookOut " + name);

            agent.SetBool("isLookingOut", false);
            agent.SetBool("isIdling", true);
            agent.SetBool("isFollowingOrder", false);
            agent.SetBool("isFollowing", false);

            OnStopLookOut?.Invoke();
        }
    }

    public void StopFollow()
    {
        if (!isDead)
        {
            Debug.Log("[Sheep] Stop Follow " + name);

            agent.SetBool("isFollowing", false);
            agent.SetBool("isLookingOut", false);
            agent.SetBool("isIdling", true);
            agent.SetBool("isFollowingOrder", false);

            OnStopFollow?.Invoke();
        }
    }

    public bool IsFollowingOrder() { return agent.GetBool("isFollowingOrder"); }
    public bool IsFollowing() { return agent.GetBool("isFollowing"); }
    public bool IsLookingOut() { return agent.GetBool("isLookingOut"); }
}
