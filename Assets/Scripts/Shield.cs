using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Material shieldMAT;
    private bool isHit = false;
    [SerializeField] private float hitFlashTime = 0.5f;
    private float hitTimer = 0f;

    private void Start()
    {
        shieldMAT.SetInt("IsHit_", 0);
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit) {
            hitTimer += Time.deltaTime;
            if (hitTimer > hitFlashTime)
            {
                shieldMAT.SetInt("IsHit_", 0);
                isHit = false;
                hitTimer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        shieldMAT.SetInt("IsHit_", 1);
        isHit = true;
    }

}
