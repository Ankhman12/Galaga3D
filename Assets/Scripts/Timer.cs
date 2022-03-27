using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public int startMinutes;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timerActive = false;
                //Start();
            }
        }
    }
    public void StartTimer()
    {
        timerActive = true;
    }
    
    public void StopTimer()
    {
        timerActive = false;
    }
    public void ResetTimer()
    {
        currentTime = startMinutes * 60;
    }

    public bool GetTimerActive()
    {
        return timerActive;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public string PrintCurrentTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return time.ToString(@"mm\:ss\:fff");
    }
}
