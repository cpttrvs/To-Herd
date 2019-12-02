using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    [SerializeField]
    private SheepSelector sheepSelector = null;

    [SerializeField]
    private float speed = 3f;

    private bool hasAMoveOrder = false;
    private Vector3 currentMovePosition = Vector3.zero;

    private void Start()
    {
        sheepSelector.OnSelection += SheepSelector_OnSelection;
        sheepSelector.OnDeselection += SheepSelector_OnDeselection;
        sheepSelector.OnMoveOrder += SheepSelector_OnMoveOrder;
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentMovePosition.x, transform.position.y, currentMovePosition.z), speed * Time.deltaTime);

            if(transform.position.x == currentMovePosition.x && transform.position.z == currentMovePosition.z)
            {
                hasAMoveOrder = false;
                currentMovePosition = Vector3.zero;
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
    }
}
