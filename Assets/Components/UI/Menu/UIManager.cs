using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public Action OnPlay;
    public Action OnQuit;
    
    [SerializeField]
    private GameObject gameConfigPrefab = null;

    [SerializeField]
    private Button playButton = null;
    [SerializeField]
    private Toggle settingsToggle = null;
    [SerializeField]
    private Button quitButton = null;

    [Header("Settings")]
    [SerializeField]
    private RectTransform settingsPanel = null;
    [SerializeField]
    private Toggle timerToggle = null;
    [SerializeField]
    private TextMeshProUGUI nbSheeps = null;
    [SerializeField]
    private Button addSheeps = null;
    [SerializeField]
    private Button removeSheeps = null;

    [Header("Game")]
    [SerializeField]
    private int minSheeps = 1;
    [SerializeField]
    private int maxSheeps = 25;
    private int numberOfSheeps = 10;

    private GameConfig gameConfig = null;

    private void Start()
    {
        settingsPanel.gameObject.SetActive(false);

        gameConfig = FindObjectOfType<GameConfig>();
        if(gameConfig == null)
        {
            Debug.LogWarning("[UIManager] no game config found, creating");
            GameObject temp = Instantiate(gameConfigPrefab);
            gameConfig = temp.GetComponentInChildren<GameConfig>();
        }

        numberOfSheeps = gameConfig.nbSheeps;
        nbSheeps.text = numberOfSheeps.ToString();
        timerToggle.isOn = gameConfig.showTimer;


        timerToggle.onValueChanged.AddListener(OnTimerValueChanged);
        addSheeps.onClick.AddListener(OnAddSheepClick);
        removeSheeps.onClick.AddListener(OnRemoveSheepClick);

        playButton.onClick.AddListener(OnPlayClick);
        settingsToggle.onValueChanged.AddListener(OnSettingsValueChanged);
        quitButton.onClick.AddListener(OnQuitClick);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayClick);
        settingsToggle.onValueChanged.RemoveListener(OnSettingsValueChanged);
        quitButton.onClick.RemoveListener(OnQuitClick);
    }

    void OnPlayClick()
    {
        gameConfig.showTimer = timerToggle.isOn;
        gameConfig.nbSheeps = numberOfSheeps;

        OnPlay?.Invoke();
    }

    void OnSettingsValueChanged(bool v)
    {
        settingsPanel.gameObject.SetActive(v);
    }

    void OnQuitClick()
    {
        OnQuit?.Invoke();
        Application.Quit();
    }

    void OnTimerValueChanged(bool v)
    {

    }

    void OnAddSheepClick()
    {
        if(numberOfSheeps < maxSheeps)
        {
            numberOfSheeps++;
            nbSheeps.text = numberOfSheeps.ToString();
        }
    }

    void OnRemoveSheepClick()
    {
        if(numberOfSheeps > minSheeps)
        {
            numberOfSheeps--;
            nbSheeps.text = numberOfSheeps.ToString();
        }
    }
}
