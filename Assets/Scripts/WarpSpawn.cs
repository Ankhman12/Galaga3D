using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpawn : MonoBehaviour
{
    public float entrySpeed;
    public GameObject warpVFX;
    public float entryTime;
    float entryTimer = 0f;


    void Awake() { 
        Instantiate(warpVFX, this.transform);
    }

    void Update()
    {
        if (entryTimer < entryTime)
        {
            Vector3 pos = this.transform.position;
            pos += this.transform.forward * entrySpeed * Time.deltaTime;
            this.transform.position = pos;
        }
        entryTimer += Time.deltaTime;

    }
}
