using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monolith : MonoBehaviour
{
    public Action OnSheepEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sheep"))
        {
            Debug.Log("[Monolith] Sheep entered");
            OnSheepEnter?.Invoke();
        }
    }
}
