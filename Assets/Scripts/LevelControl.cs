using UnityEngine;

public class LevelControl : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // If not on the menu
        if (!GameManager.onMenu)
        {
            // If playing level-based
            if (!GameManager.levelEndless)
            {
                // If the game is ongoing
                if (!GameManager.endOfGame)
                {
                    // If we are going to the next level
                    if (GameManager.startReset)
                    {
                        // Make sure the level doesn't progress while setting up next level
                        GameManager.levelPassed = true;
                        LevelTransition();
                    }
                    else
                    {
                        GameManager.levelPassed = false;
                    }
                    
                    // If all the asteroids have been destroyed, start level transition
                    if (GameManager.asteroidSpawner.transform.childCount == 0)
                    {
                        if (GameManager.level == 3)
                        {
                            GameManager.endOfGame = true;
                        }
                        GameManager.levelPassed = true;
                    }
                    // If the spaceship has collided with an asteroid, end the game
                    if (GameManager.collided)
                    {
                        GameManager.endOfGame = true;
                    }
                }
                // Initiate end game sequence
                else
                {
                    EndGame();
                }
            }
            // If playing endless
            else
            {
                if (!GameManager.endOfGame)
                {
                    // Pretty much only check if the spaceship has collided
                    if (GameManager.collided)
                    {
                        GameManager.endOfGame = true;
                    }
                }
                else
                {
                    EndGame();
                }
            }
        }
        else
        {
            /*
            if (endlessSelected)
            {
                Globals.levelEndless = true;
            }
            else
            {
                Globals.levelEndless = false;
            }
            Globals.onMenu = false;            
            */
        }
    }
    
    // Sets up the next level
    void LevelTransition()
    {
        // DisplayPoints
        //AsteroidHandler.CreateAsteroids();
        //GameManager.timer = 500;
        GameManager.level += 1;
        GameManager.startReset = false;
        GameManager.levelPassed = false;
    }
    
    // Controls the end game sequence
    void EndGame()
    {
        // If collided or ran out of time, display game over screen
        if (GameManager.collided || GameManager.ranOut)
        {
            // DisplayGameOver
        }
        // If finished the game, display success screen
        else
        {
            // DisplaySuccess
        }
        // DisplayPoints
        // DisplayHighScore
        // LevelSelection: Retry or Back to Main Menu
        /**
        if (retrySelected)
        {
            if (!Globals.levelEndless)
            {
                Globals.level = 0;
                Globals.timer = 500;
                AsteroidHandler.CreateAsteroids();
            }
            Globals.endOfGame = false;
            Globals.levelPassed = false;
        }
        else
        {
            Globals.onMenu = true;
        }
        */
    }
        
}
