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

    [SerializeField]
    private Herd herd = null;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI currentNbSheeps = null;
    [SerializeField]
    private TextMeshProUGUI maxNbSheeps = null;

    [SerializeField]
    private GameObject timerObject = null;
    [SerializeField]
    private TextMeshProUGUI txtTimer = null;



    private float timeSinceStart = 0;
    private GameConfig gameConfig = null;

    private void Start()
    {
        gameConfig = FindObjectOfType<GameConfig>();

        if(gameConfig == null)
        {
            Debug.LogError("[GameManager] game config not found");
        } else
        {
            timerObject.SetActive(gameConfig.showTimer);

            herd.Init();
            herd.AddSheeps(gameConfig.nbSheeps);

            maxNbSheeps.text = herd.GetNbSheeps().ToString();

        }
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
        txtTimer.text = (timeSinceStart).ToString("0.0");

        currentNbSheeps.text = enclosure.GetNumberOfSheepsInside().ToString();
    }
}
