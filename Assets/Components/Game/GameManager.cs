using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject monolithPrefab = null;

    private Monolith monolith = null;

    // Start is called before the first frame update
    void Start()
    {
        if(monolithPrefab != null)
        {
            monolith = monolithPrefab.GetComponentInChildren<Monolith>();

            if(monolith != null)
            {
                monolith.OnSheepEnter += Monolith_OnSheepEnter;
            }
        }
    }

    private void OnDestroy()
    {
        if (monolith != null)
        {
            monolith.OnSheepEnter -= Monolith_OnSheepEnter;
        }
    }

    void Monolith_OnSheepEnter()
    {
        // end game
    }
}
