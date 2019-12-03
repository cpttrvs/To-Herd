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

    [SerializeField]
    private float speed = 3f;

    private bool hasAMoveOrder = false;
    private Vector3 currentMovePosition = Vector3.zero;

    private void Start()
    {
        sheepSelector.OnSelection += SheepSelector_OnSelection;
        sheepSelector.OnDeselection += SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder += SheepSelector_OnMoveOrder;

        animator.SetBool("isMoving", false);
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
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentMovePosition.x, transform.position.y, currentMovePosition.z), speed * Time.deltaTime);
            navMeshAgent.SetDestination(currentMovePosition);

            if(transform.position.x == currentMovePosition.x && transform.position.z == currentMovePosition.z)
            {
                hasAMoveOrder = false;
                currentMovePosition = Vector3.zero;
                animator.SetBool("isMoving", false);
            }
        }
    }



    void SheepSelector_OnSelection()
    {
        Debug.Log("OnSelection");
    }

    void SheepSelector_OnDeselection()
    {
        Debug.Log("OnDeselection");
    }

    void SheepSelector_OnMoveOrder(Vector3 pos)
    {
        Debug.Log("OnMove " + pos);
        hasAMoveOrder = true;
        currentMovePosition = pos;
        animator.SetBool("isMoving", true);
    }
}
