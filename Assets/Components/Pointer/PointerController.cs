using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerController : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private LayerMask selectionLayer = 0;
    [SerializeField]
    private LayerMask terrainLayer = 0;
    [SerializeField]
    private LayerMask uiLayer = 0;

    [SerializeField]
    private WolfSelector currentSelectedWolf = null;

    private WolfSelector[] selectors = null;
    
    void Start()
    {
        cam = Camera.main;

        selectors = FindObjectsOfType<WolfSelector>();
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i].OnSelection += Wolf_OnSelection;
            selectors[i].OnDeselection += Wolf_OnDeselection;
        }
    }
    
    void Update()
    {
        DebugRay();
        
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("left");

            //prevent clicking on UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                SelectWolf();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("right");
            MoveWolf();
        }

    }

    void Wolf_OnSelection(WolfSelector w)
    {
        currentSelectedWolf = w;
    }
    void Wolf_OnDeselection(WolfSelector w)
    {
        currentSelectedWolf = null;
    }

    void SelectWolf()
    {
        // raycast from mouse position in camera
        Ray rayClick = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        
        if(Physics.Raycast(rayClick, out raycastHit, Mathf.Infinity, selectionLayer.value))
        {
            Debug.DrawRay(rayClick.origin, rayClick.direction * 100, Color.red);

            WolfSelector wolfSelector = raycastHit.transform.GetComponentInChildren<WolfSelector>();

            if(wolfSelector == null)
            {
                Debug.Log("[PointerController] SheepSelector not found in " + raycastHit.transform.name);
            } else
            {
                if(currentSelectedWolf != wolfSelector)
                {
                    Debug.Log("[PointerController] Sheep selected: " + raycastHit.transform.name);

                    if (currentSelectedWolf != null)
                    {
                        currentSelectedWolf.Deselect();
                    }

                    currentSelectedWolf = wolfSelector;

                    currentSelectedWolf.Select();
                } else
                {
                    Debug.Log("[PointerController] Same sheep selected: " + raycastHit.transform.name);
                }
            }
        } else
        {
            /*
            Debug.DrawRay(rayClick.origin, rayClick.direction * 100, Color.yellow);

            Debug.Log("[PointerController] No sheep selected");

            if(currentSelectedWolf != null)
            {
                currentSelectedWolf.Deselect();
                currentSelectedWolf = null;
            }
            */
        }

    }

    void MoveWolf()
    {
        if(currentSelectedWolf != null)
        {
            // raycast from mouse position in camera
            Ray rayClick = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            // raycast in selectionLayer
            if (Physics.Raycast(rayClick, out raycastHit, 100f, terrainLayer))
            {
                currentSelectedWolf.Move(raycastHit.point);
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
