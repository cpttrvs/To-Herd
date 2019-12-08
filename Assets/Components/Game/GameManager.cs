using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public Action OnRestart;
    public Action OnQuit;

    [SerializeField]
    private float timeBeforeStart = 1.5f;
    private float currentTimeBeforeStart = 0f;

    [Header("Game")]
    [SerializeField]
    private GameObject gameConfigPrefab = null;
    [SerializeField]
    private SmartCamera smartCamera = null;
    [SerializeField]
    private WolfController wolfA = null;
    [SerializeField]
    private WolfController wolfB = null;

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

    [SerializeField]
    private GameObject finishUI = null;
    [SerializeField]
    private Button restartButton = null;
    [SerializeField]
    private Button quitButton = null;
    [SerializeField]
    private string menuSceneName = "";

    private float timeSinceStart = 0;
    private GameConfig gameConfig = null;
    private bool hasStarted = false;
    private bool isInit = false;

    public void Init()
    {
        hasStarted = false;
        currentTimeBeforeStart = 0f;
        
        //ui
        finishUI.SetActive(false);

        //game
        gameConfig = FindObjectOfType<GameConfig>();

        if (gameConfig == null)
        {
            Debug.LogWarning("[GameManager] no game config found, creating");
            GameObject temp = Instantiate(gameConfigPrefab);
            gameConfig = temp.GetComponentInChildren<GameConfig>();
        }

        timerObject.SetActive(gameConfig.showTimer);

        herd.Init();
        herd.AddSheeps(gameConfig.nbSheeps);

        maxNbSheeps.text = herd.GetNbSheeps().ToString();
        currentNbSheeps.text = enclosure.GetNumberOfSheepsInside().ToString();

        smartCamera.Init();

        isInit = true;
    }

    private void Update()
    {
        if(isInit)
        {
            if (!hasStarted)
            {
                currentTimeBeforeStart += Time.deltaTime;
                if (currentTimeBeforeStart >= timeBeforeStart)
                {
                    // start
                    wolfA.GetComponentInChildren<WolfSelector>().Select();

                    hasStarted = true;
                }
            }
            else
            {
                timeSinceStart += Time.deltaTime;
                txtTimer.text = (timeSinceStart).ToString("0.0");

                currentNbSheeps.text = enclosure.GetNumberOfSheepsInside().ToString();

                // if all the sheeps are inside, win
                if (enclosure.GetNumberOfSheepsInside() == herd.GetNbSheeps())
                {
                    restartButton.onClick.AddListener(OnRestartClick);
                    quitButton.onClick.AddListener(OnQuitClick);

                    finishUI.SetActive(true);

                    Time.timeScale = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    void OnRestartClick()
    {
        restartButton.onClick.RemoveListener(OnRestartClick);

        OnRestart?.Invoke();
    }

    void OnQuitClick()
    {
        quitButton.onClick.RemoveListener(OnQuitClick);

        OnQuit?.Invoke();
    }
}
