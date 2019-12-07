using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WolfSelector : MonoBehaviour
{
    public bool isSelected = false;

    public Action<WolfSelector> OnSelection;
    public Action<WolfSelector> OnDeselection;

    public Action<Vector3> OnMoveOrder;

    [SerializeField]
    private WolfController _controller = null;

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

    public WolfController GetController() { return _controller; }
}
