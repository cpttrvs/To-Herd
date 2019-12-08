using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "";

    [SerializeField]
    private Button playButton = null;
    [SerializeField]
    private Button settingsButton = null;
    [SerializeField]
    private Button quitButton = null;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClick);
        settingsButton.onClick.AddListener(OnSettingsClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayClick);
        settingsButton.onClick.RemoveListener(OnSettingsClick);
        quitButton.onClick.RemoveListener(OnQuitClick);
    }

    void OnPlayClick()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    void OnSettingsClick()
    {

    }

    void OnQuitClick()
    {
        Application.Quit();
    }
}
