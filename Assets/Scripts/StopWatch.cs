using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StopWatch : MonoBehaviour
{
    bool stopWatchActive = false;
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopWatchActive)
        {
            currentTime += Time.deltaTime;
        }
    }
    public void StartStopWatch()
    {
        stopWatchActive = true;
    }

    public void StopStopWatch()
    {
        stopWatchActive = false;
    }

    public void ResetStopWatch()
    {
        currentTime = 0;
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
