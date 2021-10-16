using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Scripts
    private PlayerController _playerController;
    private SpawnEnemies _spawnEnemies;
    
    //UI
    public List<RectTransform> allUiList;
    public List<RectTransform> allPauseList;
    public List<RectTransform> allGameOverList;
    public TextMeshProUGUI orbsCounterText;
    public TextMeshProUGUI orbsWavesText;
    public TextMeshProUGUI controlsText;
    
    //Bool
    private bool _inGame;
    
    //Post Processing stuff
    public PostProcessVolume postProcessVolume;
    private Vignette _vignette;
    private float _vignetteStartSmoothness;
    [Range(0, 1f), SerializeField] private float vignetteSmoothnessOnTab;

    private void Awake()
    {
        if(GameObject.Find("Player") != null)
            _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (GetComponent<SpawnEnemies>() != null)
            _spawnEnemies = GetComponent<SpawnEnemies>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        
        if(GameObject.Find("ControlsText") != null)
            StartCoroutine(HideControlsText(6));


        postProcessVolume.profile.TryGetSettings(out _vignette);
        _vignetteStartSmoothness = _vignette.smoothness.value;
    }

    private void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
            case 2:
                _inGame = false;
                break;
            
            case 1:
                _inGame = true;
                break;
        }

        //If you click TAB then the game will pause
        if (Input.GetKeyDown(KeyCode.Tab) && _inGame)
        {
            _vignette.smoothness.value = vignetteSmoothnessOnTab;
            
            Time.timeScale = 0;
            UISetOff();
            PauseSetOn();
        }
        
        if (GameObject.Find("Player") != null)
        {
            UpdateScore();
            TeleportOnOrbValue();
        }

        if (GameObject.Find("Player") != null && _playerController.orbsCounter < 0)
        {
            _vignette.smoothness.value = vignetteSmoothnessOnTab;
            Time.timeScale = 0;
            UISetOff();
            GameOverTextOn();
        }
        
        if (GetComponent<SpawnEnemies>() != null && _spawnEnemies.orbsWaves == 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale != 0)
        {
            RetryGame();
        }
    }
    
    private void TeleportOnOrbValue()
    {
        switch (_playerController.orbsCounter)
        {
            case 20:
            case 40:
            case 60:
            case 80:
            case 100:
            case 120:
            case 140:
            case 160:
            case 180:
            case 200:
                _playerController.canTeleport = true;
                break;
        }
    }

    private void GameOverTextOn()
    {
        foreach (var gameOver in allGameOverList)
        {
            gameOver.gameObject.SetActive(true);
        }
    }

    private void UISetOff()
    {
        foreach (var ui in allUiList)
        {
            ui.gameObject.SetActive(false);
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        _vignette.smoothness.value = _vignetteStartSmoothness;
        
        foreach (var ui in allUiList)
        {
            ui.gameObject.SetActive(true);
        }
        
        foreach (var pause in allPauseList)
        {
            pause.gameObject.SetActive(false);
        }
    }

    private void PauseSetOn()
    {
        foreach (var pause in allPauseList)
        {
            pause.gameObject.SetActive(true);
        }
    }
    
    private void UpdateScore()
    {
        orbsCounterText.text = _playerController.orbsCounter.ToString();
        orbsWavesText.text = _spawnEnemies.orbsWaves.ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void EnterTutorialScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void QuitTutorialScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HideControlsText(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        controlsText.gameObject.SetActive(false);
    }
}
