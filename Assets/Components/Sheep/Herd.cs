using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField]
    private GameObject sheepPrefab = null;

    private List<SheepController> sheeps = null;

    [Header("Starting box")]
    [SerializeField]
    private Vector3 center = Vector3.zero;
    [SerializeField]
    private Vector3 size = Vector3.one;
    

    public void Init()
    {
        sheeps = new List<SheepController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + center, size);
    }

    public void AddSheeps(int n)
    {
        for(int i = 0; i < n; i++)
        {
            GameObject temp = Instantiate(sheepPrefab, transform);
            
            temp.transform.position = new Vector3(transform.position.x + Random.Range(-size.x/2, size.x/2), 1, transform.position.z + Random.Range(-size.z/2, size.z/2));
            
            SheepController tempController = temp.GetComponentInChildren<SheepController>();
            if(tempController == null)
            {
                Debug.LogError("[Herd] AddSheeps: no controller found");
            } else
            {
                sheeps.Add(tempController);
                tempController.Init();
            }
            
        }
    }

    public List<SheepController> GetSheeps() { return sheeps; }
    public int GetNbSheeps() { return sheeps.Count; }
}
