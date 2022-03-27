using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static int endlesshighScore = 0;
    public static int levelhighScore = 0;
    public static string bestTime;
    public static int points = 0;
    public static int level = 0;
    public static bool levelEndless = false;
    public static bool levelPassed = false;
    public static bool startReset = false;
    public static bool endOfGame = false;
    public static bool onMenu = true;
    public static AsteroidSpawner asteroidSpawner;
    public static int currentID = 0;
    public static bool collided = false;
    public static bool ranOut = false;
    public static bool gameIsPaused = false;
    public static bool retrySelected = false;
    public float spawnTime = 20;
    private float currentSpawnTime;
    private bool earnedTimeBonus = false;


    private void Awake()
    {
        endOfGame = false;
        levelEndless = FindObjectOfType<ModeSelecter>().isEndlessMode;
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
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
    }

    void Update()
    {
        PauseGame();
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
            Cursor.lockState = CursorLockMode.Confined;
            endOfGame = true;
            //Debug.Log("Player has lost level");
            // DisplayGameOver
            if (levelEndless)
            {
                FindObjectOfType<StopWatch>().StopStopWatch();
                // Add Point bonus to points
                if (!earnedTimeBonus)
                {
                    points += (int)FindObjectOfType<StopWatch>().GetCurrentTime();
                    earnedTimeBonus = true;
                }
            }
            else
            {
                FindObjectOfType<Timer>().StopTimer();
                // Add Point bonus to points
                if (!earnedTimeBonus)
                {
                    points += (int)FindObjectOfType<Timer>().GetCurrentTime();
                    earnedTimeBonus = true;
                }
            }
            if (levelEndless && points > endlesshighScore)
            {
                endlesshighScore = points;
                bestTime = FindObjectOfType<StopWatch>().PrintCurrentTime();
            }
            else if (!levelEndless && points > levelhighScore)
            {
                levelhighScore = points;
                bestTime = FindObjectOfType<Timer>().PrintCurrentTime();
            }

            FindObjectOfType<UIManager>().hideUI();
            FindObjectOfType<UIManager>().hideWin();
            FindObjectOfType<UIManager>().showGameOver();
            if (FindObjectOfType<ShipMovement>() != null)
            {
                FindObjectOfType<ShipMovement>().enabled = false;
                FindObjectOfType<ShipShooting>().enabled = false;
            }
            //Debug.Log("GAME OVER");

        }
        //WIN CONDITION
        // If finished the game, display success screen
        else if (asteroidSpawner.transform.childCount == 0 && !levelEndless)
        {
            Cursor.lockState = CursorLockMode.Confined;
            levelPassed = true;
            endOfGame = true;
            FindObjectOfType<ShipShooting>().firing = false;
            //Debug.Log("Player has won level");
            FindObjectOfType<Timer>().StopTimer();
            // Add Point bonus to points
            if (!earnedTimeBonus)
            {
                points += (int)FindObjectOfType<Timer>().GetCurrentTime();
                earnedTimeBonus = true;
            }
            
            // DisplaySuccess if level is complete
            if (points > levelhighScore)
            {
                levelhighScore = points;
                bestTime = FindObjectOfType<Timer>().PrintCurrentTime();
            }
            FindObjectOfType<UIManager>().hideUI();
            FindObjectOfType<UIManager>().hideGameOver();
            FindObjectOfType<UIManager>().showWin();
            if (FindObjectOfType<ShipMovement>() != null)
            {
                FindObjectOfType<ShipMovement>().enabled = false;
                FindObjectOfType<ShipShooting>().enabled = false;
            }
            // Transition to Next Level
        }
        // DisplayPoints
        // DisplayHighScore
        // LevelSelection: Retry or Back to Main Menu


    }

    void Restart()
    {
        foreach (Transform child in asteroidSpawner.transform)
        {
            GameObject.Destroy(child.gameObject);
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
        points = 0;
        endOfGame = false;
        levelPassed = false;
        collided = false;
        ranOut = false;
        retrySelected = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void PauseGame()
    {
        if (gameIsPaused && !endOfGame)
        {
            //Debug.Log("PAUSED");
            //Display Pause Menu
            FindObjectOfType<UIManager>().showPaused();
            FindObjectOfType<UIManager>().hideUI();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
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
}
