using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    private string gameScene = "";
    [SerializeField]
    private string menuScene = "";

    [SerializeField]
    private Camera cam = null;
    [SerializeField]
    private GameObject canvas = null;

    private UIManager menuManager = null;
    private GameManager gameManager = null;

    private bool mustLoadGame = false;

    private void Awake()
    {
        mustLoadGame = false;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == menuScene)
        {
            cam.gameObject.SetActive(false);
            canvas.SetActive(false);

            menuManager = FindObjectOfType<UIManager>();

            menuManager.OnPlay += MenuManagerOnPlay;
            menuManager.OnQuit += MenuManagerOnQuit;
        }

        if(arg0.name == gameScene)
        {
            cam.gameObject.SetActive(false);
            canvas.SetActive(false);

            gameManager = FindObjectOfType<GameManager>();

            gameManager.OnRestart += GameManagerOnRestart;
            gameManager.OnQuit += GameManagerOnQuit;

            gameManager.Init();
        }
    }
    
    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        if(mustLoadGame)
        {
            SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
        } else
        {
            SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);
        }
    }

    void MenuManagerOnPlay()
    {
        menuManager.OnPlay -= MenuManagerOnPlay;
        menuManager.OnQuit -= MenuManagerOnQuit;
        
        canvas.SetActive(true);
        cam.gameObject.SetActive(true);
        mustLoadGame = true;
        SceneManager.UnloadSceneAsync(menuScene);
    }

    void MenuManagerOnQuit()
    {
        menuManager.OnPlay -= MenuManagerOnPlay;
        menuManager.OnQuit -= MenuManagerOnQuit;

        mustLoadGame = false;
        Application.Quit();
    }

    void GameManagerOnRestart()
    {
        gameManager.OnRestart -= GameManagerOnRestart;
        gameManager.OnQuit -= GameManagerOnQuit;

        canvas.SetActive(true);
        cam.gameObject.SetActive(true);
        mustLoadGame = true;
        SceneManager.UnloadSceneAsync(gameScene);
    }

    void GameManagerOnQuit()
    {
        gameManager.OnRestart -= GameManagerOnRestart;
        gameManager.OnQuit -= GameManagerOnQuit;

        canvas.SetActive(true);
        cam.gameObject.SetActive(true);
        mustLoadGame = false;
        SceneManager.UnloadSceneAsync(gameScene);
    }
}
