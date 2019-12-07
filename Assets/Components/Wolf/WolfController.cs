using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class WolfController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private WolfSelector selector = null;

    public Action OnSelect;
    public Action OnDeselect;
    public Action OnMoveOrder;

    private bool isSelected = false;

    private Vector3 lastPosition = Vector3.zero;

    private void Start()
    {
        selector.OnSelection += WolfSelector_OnSelection;
        selector.OnDeselection += WolfSelector_OnDeselection;
        selector.OnMoveOrder += WolfSelector_OnMoveOrder;

        animator.SetBool("isIdling", true);
        lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        selector.OnSelection -= WolfSelector_OnSelection;
        selector.OnDeselection -= WolfSelector_OnDeselection;
        selector.OnMoveOrder -= WolfSelector_OnMoveOrder;
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

    void WolfSelector_OnSelection(WolfSelector s)
    {
        Debug.Log("[Wolf] OnSelection: " + name);
        isSelected = true;
        OnSelect?.Invoke();
    }

    void WolfSelector_OnDeselection(WolfSelector s)
    {
        Debug.Log("[Wolf] OnDeselection: " + name);
        isSelected = false;
        OnDeselect?.Invoke();
    }

    void WolfSelector_OnMoveOrder(Vector3 pos)
    {
        Debug.Log("[Wolf] OnMove: " + pos + ", " + name);

        OnMoveOrder?.Invoke();

        navMeshAgent.SetDestination(pos);
        Debug.DrawLine(transform.position, pos, Color.red);

    }
}
