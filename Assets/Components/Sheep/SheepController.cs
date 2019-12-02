using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    [SerializeField]
    private SheepSelector sheepSelector = null;

    private bool hasAMoveOrder = false;

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
    }
}
