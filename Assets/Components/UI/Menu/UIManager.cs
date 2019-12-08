using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "";

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
    [SerializeField]
    private int numberOfSheeps = 10;

    private GameConfig gameConfig = null;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClick);
        settingsToggle.onValueChanged.AddListener(OnSettingsValueChanged);
        quitButton.onClick.AddListener(OnQuitClick);

        settingsPanel.gameObject.SetActive(false);

        timerToggle.onValueChanged.AddListener(OnTimerValueChanged);
        addSheeps.onClick.AddListener(OnAddSheepClick);
        removeSheeps.onClick.AddListener(OnRemoveSheepClick);

        nbSheeps.text = numberOfSheeps.ToString();

        gameConfig = FindObjectOfType<GameConfig>();
        if(gameConfig == null)
        {
            Debug.LogError("[UIManager] no game config found");
        }
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

        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    void OnSettingsValueChanged(bool v)
    {
        settingsPanel.gameObject.SetActive(v);
    }

    void OnQuitClick()
    {
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
