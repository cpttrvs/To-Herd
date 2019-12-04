using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomCollider : MonoBehaviour
{
    public Action<Collision> OnCollisionEntered;
    public Action<Collision> OnCollisionExited;
    public Action<Collision> OnCollisionStayed;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEntered?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExited?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayed?.Invoke(collision);
    }
}
