using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    SheepSelector[] selectors = null;

    
    public GameObject currentSheep = null;

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
        selectors = FindObjectsOfType<SheepSelector>();
        for(int i = 0; i < selectors.Length; i++)
        {
            selectors[i].OnSelection += Sheep_OnSelection;
            selectors[i].OnDeselection += Sheep_OnDeselection;
        }
    }

    private void Update()
    {
        if(currentSheep != null)
        {
            Vector3 desiredPosition = currentSheep.transform.position + offset;
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
                selectors[i].OnSelection -= Sheep_OnSelection;
                selectors[i].OnDeselection -= Sheep_OnDeselection;
            }
        }
    }

    void Sheep_OnSelection(SheepSelector s)
    {
        currentSheep = s.gameObject;
    }

    void Sheep_OnDeselection(SheepSelector s)
{

    }
}
