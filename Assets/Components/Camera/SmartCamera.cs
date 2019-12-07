using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    WolfSelector[] selectors = null;

    
    public GameObject currentWolf = null;

    [SerializeField]
    private float smoothSpeed = 0.125f;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        selectors = FindObjectsOfType<WolfSelector>();
        for(int i = 0; i < selectors.Length; i++)
        {
            selectors[i].OnSelection += Wolf_OnSelection;
            selectors[i].OnDeselection += Wolf_OnDeselection;
        }
    }

    private void Update()
    {
        if(currentWolf != null)
        {
            Vector3 desiredPosition = currentWolf.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private void OnDestroy()
    {
        if(selectors != null)
        {
            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].OnSelection -= Wolf_OnSelection;
                selectors[i].OnDeselection -= Wolf_OnDeselection;
            }
        }
    }

    void Wolf_OnSelection(WolfSelector s)
    {
        currentWolf = s.gameObject;
    }

    void Wolf_OnDeselection(WolfSelector s)
{

    }
}
