using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private Enclosure enclosure = null;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI txtNbEnclosure = null;
    [SerializeField]
    private TextMeshProUGUI txtTimer = null;


    private float timeSinceStart = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
        txtTimer.text = ((int)timeSinceStart).ToString();

        txtNbEnclosure.text = enclosure.GetNumberOfSheepsInside().ToString();
    }
}
