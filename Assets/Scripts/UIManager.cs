using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject[] pausedObjects;
    public GameObject[] gameOverObjects;
    public GameObject[] playerUIObjects;
    public GameObject[] winObjects;

    [SerializeField] private ShipMovement shipMovement;
    [SerializeField] private ShipShooting shipShooting;

    //Player UI: Current Score Text
    [SerializeField] private TextMeshProUGUI currentScoreText;
    //Player UI: High Score Text
    [SerializeField] private TextMeshProUGUI highScoreText;
    //Player UI: Timer Text
    [SerializeField] private TextMeshProUGUI timerText;
    //Player UI: Lives Image
    [SerializeField] private GameObject[] livesImages;
    //Player UI: Boost Bar
    [SerializeField] private Image boostBarImage;
    //Player UI: Blaster Crosshair
    [SerializeField] private GameObject blasterCrosshairImage;
    //Player UI: Laser Crosshair
    [SerializeField] private GameObject laserCrosshairImage;

    //Pause: Restart Button
    [SerializeField] private Button pauseRestartButton;
    //Pause: Exit Game Button
    [SerializeField] private Button pauseExitButton;
    //Pause: Exit Mode Button
    [SerializeField] private Button pauseExitModeButton;

    //Game Over: Restart Button
    [SerializeField] private Button gameOverRestartButton;
    //Game Over: Exit Game Button
    [SerializeField] private Button gameOverExitButton;
    //Game Over: Exit Mode Button
    [SerializeField] private Button gameOverExitModeButton;
    //Game Over: Scoreboard Text
    [SerializeField] private TextMeshProUGUI gameOverScoreboardText;

    //Win: Next Level Button
    [SerializeField] private Button winNextLevelButton;
    //Win: Exit Game Button
    [SerializeField] private Button winExitButton;
    //Win: Exit Mode Button
    [SerializeField] private Button winExitModeButton;
    //Win: Scoreboard Text
    [SerializeField] private TextMeshProUGUI winScoreboardText;

    private void Awake()
    {
        pauseExitButton.onClick.AddListener(ExitOnClick);
        gameOverExitButton.onClick.AddListener(ExitOnClick);
        winExitButton.onClick.AddListener(ExitOnClick);

        pauseExitModeButton.onClick.AddListener(ExitModeOnClick);
        gameOverExitModeButton.onClick.AddListener(ExitModeOnClick);
        winExitModeButton.onClick.AddListener(ExitModeOnClick);

        pauseRestartButton.onClick.AddListener(RestartOnClick);
        gameOverRestartButton.onClick.AddListener(RestartOnClick);

        winNextLevelButton.onClick.AddListener(NextLevelOnClick);
    }

    

    private void Start()
    {
        shipMovement = FindObjectOfType<ShipMovement>();
        shipShooting = FindObjectOfType<ShipShooting>();
    }

    private void Update()
    {
        if (shipMovement != null)
        {
            if (shipShooting.fireMode == ShipShooting.FireMode.Blaster)
            {
                blasterCrosshairImage.SetActive(true);
                laserCrosshairImage.SetActive(false);
            }
            else if (shipShooting.fireMode == ShipShooting.FireMode.Laser)
            {
                blasterCrosshairImage.SetActive(false);
                laserCrosshairImage.SetActive(true);
            }
            currentScoreText.text = "Score: " + GameManager.points;
            if (GameManager.levelEndless)
            {
                highScoreText.text = "High Score: " + GameManager.endlesshighScore;
            }
            else
            {
                highScoreText.text = "High Score: " + GameManager.levelhighScore;
            }
            if (GameManager.levelEndless)
            {
                timerText.text = FindObjectOfType<StopWatch>().PrintCurrentTime();
            }
            else
            {
                timerText.text = FindObjectOfType<Timer>().PrintCurrentTime();
            }
            boostBarImage.fillAmount = shipMovement.currentBoostAmount / shipMovement.maxBoostAmount;
            UpdateLives();
        }
        if (GameManager.endOfGame)
        {
            if (GameManager.levelEndless)
            {
                gameOverScoreboardText.text =
                "High Score: " + GameManager.endlesshighScore +
                "\nBest Time: " + GameManager.bestTime +
                "\nCurrent Score: " + GameManager.points +
                "\nCurrent Time: " + FindObjectOfType<StopWatch>().PrintCurrentTime();
            }
            else
            {
                gameOverScoreboardText.text =
                "High Score: " + GameManager.levelhighScore +
                "\nBest Time: " + GameManager.bestTime +
                "\nCurrent Score: " + GameManager.points +
                "\nCurrent Time: " + FindObjectOfType<Timer>().PrintCurrentTime();
            }
        }
        if (GameManager.levelPassed)
        {
            winScoreboardText.text =
                "High Score: " + GameManager.levelhighScore +
                "\nBest Time: " + GameManager.bestTime +
                "\nCurrent Score: " + GameManager.points +
                "\nCurrent Time: " + FindObjectOfType<Timer>().PrintCurrentTime();
        }
    }

    private void RestartOnClick()
    {
        GameManager.retrySelected = true;
    }

    public void ExitOnClick()
    {
        Debug.Log("Exit Button clicked");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void ExitModeOnClick()
    {
        GameManager.points = 0;
        GameManager.endOfGame = false;
        GameManager.levelPassed = false;
        GameManager.collided = false;
        GameManager.ranOut = false;
        SceneManager.LoadScene(0);
    }

    private void NextLevelOnClick()
    {
        GameManager.points = 0;
        GameManager.endOfGame = false;
        GameManager.levelPassed = false;
        GameManager.collided = false;
        GameManager.ranOut = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateLives()
    {
        switch (shipMovement.currentLives)
        {
            case 1:
                livesImages[0].SetActive(true);
                livesImages[1].SetActive(false);
                livesImages[2].SetActive(false);
                livesImages[3].SetActive(false);
                livesImages[4].SetActive(false);
                break;
            case 2:
                livesImages[0].SetActive(true);
                livesImages[1].SetActive(true);
                livesImages[2].SetActive(false);
                livesImages[3].SetActive(false);
                livesImages[4].SetActive(false);
                break;
            case 3:
                livesImages[0].SetActive(true);
                livesImages[1].SetActive(true);
                livesImages[2].SetActive(true);
                livesImages[3].SetActive(false);
                livesImages[4].SetActive(false);
                break;
            case 4:
                livesImages[0].SetActive(true);
                livesImages[1].SetActive(true);
                livesImages[2].SetActive(true);
                livesImages[3].SetActive(true);
                livesImages[4].SetActive(false);
                break;
            case 5:
                livesImages[0].SetActive(true);
                livesImages[1].SetActive(true);
                livesImages[2].SetActive(true);
                livesImages[3].SetActive(true);
                livesImages[4].SetActive(true);
                break;
            default:
                break;
        }
    }

    #region Pause Methods
    public void showPaused()
    {
        foreach (var g in pausedObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach (var g in pausedObjects)
        {
            g.SetActive(false);
        }
    }
    #endregion
    #region Game Over Methods
    public void showGameOver()
    {
        foreach (var g in gameOverObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideGameOver()
    {
        foreach (var g in gameOverObjects)
        {
            g.SetActive(false);
        }
    }
    #endregion
    #region UI Methods
    public void showUI()
    {
        foreach (var g in playerUIObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideUI()
    {
        foreach (var g in playerUIObjects)
        {
            g.SetActive(false);
        }
    }
    #endregion
    #region Win Methods
    public void showWin()
    {
        foreach (var g in winObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideWin()
    {
        foreach (var g in winObjects)
        {
            g.SetActive(false);
        }
    }
    #endregion
}
