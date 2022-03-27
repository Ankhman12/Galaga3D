using UnityEngine;

public class Points : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.onMenu)
        {
            if (!GameManager.levelEndless)
            {
                if (!GameManager.endOfGame)
                {
                    // Decrement timer on every tick
                    if (FindObjectOfType<Timer>().GetTimerActive())
                    {
                        GameManager.endOfGame = true;
                        GameManager.ranOut = true;
                    }
                    
                    // The following is testing for points
                    //if (GameManager.points == 0)
                    //{
                    //    GameManager.points += 100;
                    //} else
                    //{
                    //    GameManager.points += 25;
                    //}
                    
                    //Debug.Log("High Score: " + Globals.highScore);
                    //Debug.Log("Level: " + Globals.level);
                    //Debug.Log("Timer: " + Globals.timer);
                    
                    if (GameManager.levelPassed)
                    {
                        int levelBonus = GameManager.level * 2500;
                        int timeBonus = (int)FindObjectOfType<Timer>().GetCurrentTime() * 10;            
                        // Display both         
                        GameManager.points += (levelBonus + timeBonus);
                        Debug.Log(timeBonus);
                        GameManager.startReset = true;            
                    }
                }
            }
            else
            {        
                //if (GameManager.endOfGame)
                //{
                //    if (GameManager.points > GameManager.highScore)
                //    {
                //        GameManager.highScore = GameManager.points;
                //    }
                //    GameManager.points = 0;
                //}
            }
        }
        
    }
}
