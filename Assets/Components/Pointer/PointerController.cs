using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private LayerMask selectionLayer = 0;
    [SerializeField]
    private LayerMask terrainLayer = 0;

    private SheepSelector currentSelectedSheep = null;
    
    void Start()
    {
        cam = Camera.main;

    }
    
    void Update()
    {
        DebugRay();
        
        if (Input.GetMouseButtonDown(0))
        {
            SelectSheep();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveSheep();
        }

    }

    void SelectSheep()
    {
        // raycast from mouse position in camera
        Ray rayClick = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        // raycast in selectionLayer
        if (Physics.Raycast(rayClick, out raycastHit, Mathf.Infinity, selectionLayer))
        {
            SheepSelector sheepSelector = raycastHit.transform.gameObject.GetComponentInChildren<SheepSelector>();

            if (sheepSelector != null)
            {
                // if the sheep selected is different from the previous stored one
                if (sheepSelector != currentSelectedSheep)
                {
                    // select the new sheep, deselect the previous one
                    sheepSelector.Select();

                    if (currentSelectedSheep != null)
                    {
                        currentSelectedSheep.Deselect();
                    }

                    // replace the current sheep
                    currentSelectedSheep = sheepSelector;
                }
            }
            else
            {
                Debug.LogError("[PointerController] No SheepSelector found on " + raycastHit.transform.name);
            }
        }
        else
        {
            // deselect the current sheep if clicked somewhere else
            if (currentSelectedSheep != null)
            {
                currentSelectedSheep.Deselect();
                currentSelectedSheep = null;
            }
        }
    }

    void MoveSheep()
    {
        if(currentSelectedSheep != null)
        {
            // raycast from mouse position in camera
            Ray rayClick = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            // raycast in selectionLayer
            if (Physics.Raycast(rayClick, out raycastHit, Mathf.Infinity, terrainLayer))
            {
                currentSelectedSheep.Move(raycastHit.point);
            }
        }
    }

    void DebugRay()
    {
        // Debug ray
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, selectionLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            //Debug.Log(raycastHit.transform.name + " " + raycastHit.transform.gameObject.layer);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        }
    }
}
