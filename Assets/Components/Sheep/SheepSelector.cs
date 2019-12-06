using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SheepSelector : MonoBehaviour
{
    public bool isSelected = false;

    public Action<SheepSelector> OnSelection;
    public Action<SheepSelector> OnDeselection;

    public Action<Vector3> OnMoveOrder;

    [SerializeField]
    private SheepController _controller = null;

    public void Select()
    {
        //Debug.Log("SELECTED");
        isSelected = true;
        OnSelection?.Invoke(this);
    }

    public void Deselect()
    {
        //Debug.Log("DESELECTED");
        isSelected = false;
        OnDeselection?.Invoke(this);
    }

    public void Move(Vector3 pos)
    {
        //Debug.Log("MOVE:" + pos);
        OnMoveOrder?.Invoke(pos);
    }

    public SheepController GetController() { return _controller; }
}
