using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enclosure : MonoBehaviour
{
    [SerializeField]
    private Vector3 center = Vector3.zero;
    [SerializeField]
    private Vector3 size = Vector3.one;

    [SerializeField]
    private Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position + center, size);
    }

    public int GetNumberOfSheepsInside()
    {
        int nb = 0;
        
        Collider[] colliders = Physics.OverlapBox(transform.position + center, size/2);

        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].CompareTag("Sheep"))
            {
                nb++;
            }
        }

        return nb;
    }
}
