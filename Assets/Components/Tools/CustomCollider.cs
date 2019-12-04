using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomCollider : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private Vector3 center;

    [SerializeField]
    private Color color;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position + center, radius);
    }

    public Vector3 GetPosition() { return transform.position + center; }
    public float GetRadius() { return radius; }
}
