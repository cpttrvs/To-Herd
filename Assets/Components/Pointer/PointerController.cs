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

    [SerializeField]
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
            //Debug.Log("left");
            SelectSheep();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("right");
            MoveSheep();
        }

    }

    void SelectSheep()
    {
        // raycast from mouse position in camera
        Ray rayClick = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        
        if(Physics.Raycast(rayClick, out raycastHit, Mathf.Infinity, selectionLayer.value))
        {
            Debug.DrawRay(rayClick.origin, rayClick.direction * 100, Color.red);

            SheepSelector sheepController = raycastHit.transform.GetComponentInChildren<SheepSelector>();

            if(sheepController == null)
            {
                Debug.Log("[PointerController] SheepSelector not found in " + raycastHit.transform.name);
            } else
            {
                if(currentSelectedSheep != sheepController)
                {
                    Debug.Log("[PointerController] Sheep selected: " + raycastHit.transform.name);

                    if (currentSelectedSheep != null)
                    {
                        currentSelectedSheep.Deselect();
                    }

                    currentSelectedSheep = sheepController;

                    currentSelectedSheep.Select();
                } else
                {
                    Debug.Log("[PointerController] Same sheep selected: " + raycastHit.transform.name);
                }
            }
        } else
        {
            Debug.DrawRay(rayClick.origin, rayClick.direction * 100, Color.yellow);

            Debug.Log("[PointerController] No sheep selected");

            if(currentSelectedSheep != null)
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
            if (Physics.Raycast(rayClick, out raycastHit, 100f, terrainLayer))
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
        if (Physics.Raycast(ray, out raycastHit, 100f, selectionLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        }
    }
}
