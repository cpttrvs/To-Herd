using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SheepSelector : MonoBehaviour
{
    private bool isSelected = false;

    public Action OnSelection;
    public Action OnDeselection;

    public Action<Vector3> OnMoveOrder;

    public void Select()
    {
        Debug.Log("SELECTED");
        isSelected = true;
        OnSelection?.Invoke();
    }

    public void Deselect()
    {
        Debug.Log("DESELECTED");
        isSelected = false;
        OnDeselection?.Invoke();
    }

    public void Move(Vector3 pos)
    {
        Debug.Log("MOVE:" + pos);
        OnMoveOrder?.Invoke(pos);
    }
}
