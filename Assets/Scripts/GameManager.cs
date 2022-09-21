using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Private Variables
    private static GameManager _instance;
    [SerializeField] private static int _endlessHighScore = 0;
    [SerializeField] private GameObject hurtImage;
    private bool gameOver = false;
    public static int EndlessHighScore
    {
        get => _endlessHighScore;
        set => _endlessHighScore = value;
    }
    
    [SerializeField] private static int _points = 0;
    public static int Points
    {
        get => _points;
        set => _points = value;
    }
    
    [SerializeField] public static int level = 0;
    public int Level
    {
        get => level;
        set => level = value;
    }
    
    public static bool levelEndless = false;
    public static bool levelPassed = false;
    public static bool startReset = false;
    public static bool endOfGame = false;
    public static bool onMenu = true;
    [SerializeField] public static AsteroidSpawner asteroidSpawner;
    [SerializeField] private List<GameObject> squads;
    [SerializeField] public static int currentID = 0;
    public static int CurrentID
    {
        get => currentID;
        set => currentID = value;
    }

    public static bool collided = false;
    public static bool ranOut = false;
    public static bool gameIsPaused = false;
    public static bool retrySelected = false;
    [SerializeField] private float spawnTime = 20;
    private float currentSpawnTime;
    //private bool earnedTimeBonus = false;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        endOfGame = false;
        levelEndless = FindObjectOfType<ModeSelecter>().isEndlessMode;
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        squads = GameObject.FindGameObjectsWithTag("EnemySpawner").ToList();
        FindObjectOfType<UIManager>().hidePaused();
        FindObjectOfType<UIManager>().hideGameOver();
        FindObjectOfType<UIManager>().hideWin();
        FindObjectOfType<UIManager>().showUI();
        if (levelEndless)
        {
            FindObjectOfType<StopWatch>().StartStopWatch();
            currentSpawnTime = spawnTime;
        }
        else
        {
            FindObjectOfType<Timer>().StartTimer();
        }

        Cursor.lockState = CursorLockMode.Locked;
        _endlessHighScore = PlayerPrefs.GetInt("High Score", 0);
    }

    void Update()
    {
        //PauseGame();
        if (retrySelected)
        {
            Debug.Log("Pressed Restart");
            Invoke("Restart", 0.25f);
            if (gameIsPaused)
            {
                gameIsPaused = false;
                FindObjectOfType<UIManager>().hidePaused();
                FindObjectOfType<UIManager>().showUI();
                Time.timeScale = 1f;
            }
        }

        currentSpawnTime -= Time.deltaTime;
        if (levelEndless && currentSpawnTime <= 0)
        {
            Debug.Log("Spawning Asteroids");
            SpawnAsteroids();
            currentSpawnTime = spawnTime;
        }
    }

    private void FixedUpdate()
    {
        if (!levelEndless && FindObjectOfType<Timer>().GetCurrentTime() <= 0)
            //if (!levelEndless && !FindObjectOfType<Timer>().GetTimerActive() || FindObjectOfType<Timer>().GetCurrentTime() <= 0)
        {
            ranOut = true;
        }

        //Debug.Log("Points: " + points);
        EndGame();
    }

    // Controls the end game sequence
    void EndGame()
    {
        //LOSE CONDITION
        // If collided or ran out of time, display game over screen
        if (collided || ranOut)
        {
            foreach (var t in squads)
            {
                t.GetComponent<BoidSpawner>().DisableShooting();
            }
            Cursor.lockState = CursorLockMode.Confined;
            endOfGame = true;
            //Debug.Log("Player has lost level");
            // DisplayGameOver
            if (levelEndless)
            {
                FindObjectOfType<StopWatch>().StopStopWatch();
                // Add Point bonus to points
                /**
                if (!earnedTimeBonus)
                {
                    _points += (int) FindObjectOfType<StopWatch>().GetCurrentTime();
                    earnedTimeBonus = true;
                }
                */
            }

            if (levelEndless && _points > _endlessHighScore)
            {
                _endlessHighScore = _points;
                //_bestTime = FindObjectOfType<StopWatch>().PrintCurrentTime();
                PlayerPrefs.SetInt("High Score", _points);
            }

            FindObjectOfType<UIManager>().hideUI();
            FindObjectOfType<UIManager>().hideWin();
            FindObjectOfType<UIManager>().showGameOver();

            if (!gameOver)
            {

                GameObject.Find("GameOverMenu").GetComponentInChildren<Button>().Select();
                gameOver = true;
            }
            if (FindObjectOfType<ShipMovement>() != null)
            {
                FindObjectOfType<ShipMovement>().enabled = false;
                FindObjectOfType<ShipShooting>().enabled = false;
            }
            //Debug.Log("GAME OVER");
        }
    }

    void Restart()
    {
        foreach (Transform child in asteroidSpawner.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var squad in squads)
        {
            squad.GetComponent<BoidSpawner>().ClearEntities();
        }

        asteroidSpawner.SpawnAsteroids();
        if (levelEndless)
        {
            FindObjectOfType<StopWatch>().ResetStopWatch();
            FindObjectOfType<StopWatch>().StartStopWatch();
        }
        else
        {
            FindObjectOfType<Timer>().ResetTimer();
            FindObjectOfType<Timer>().StartTimer();
            level = 0;
        }

        _points = 0;
        endOfGame = false;
        levelPassed = false;
        collided = false;
        ranOut = false;
        retrySelected = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (gameIsPaused && !endOfGame)
        {
            //Debug.Log("PAUSED");
            //Display Pause Menu
            FindObjectOfType<UIManager>().showPaused();
            FindObjectOfType<UIManager>().hideUI();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            GameObject.Find("PauseMenu").GetComponentInChildren<Button>().Select();
            // Pause Audio
        }
        else if (!gameIsPaused && !endOfGame)
        {
            //Debug.Log("UNPAUSED");
            //Hide Pause Menu
            FindObjectOfType<UIManager>().hidePaused();
            FindObjectOfType<UIManager>().showUI();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            // Unpause Audio
        }
    }

    void SpawnAsteroids()
    {
        asteroidSpawner.SpawnAsteroids();
    }

    public void AddPoints(int val)
    {
        _points += val;
    }

    public void hurtPlayer()
    {
        StartCoroutine(PlayerHurt());
    }
    
    IEnumerator PlayerHurt()
    {
        hurtImage.SetActive(true);
        yield return new WaitForSeconds(.4f);
        hurtImage.SetActive(false);
    }
}