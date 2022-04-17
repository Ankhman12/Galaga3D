using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    private float timer = 0;

    private void Update()
    {
        if (timer >= despawnTime)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Collided with Player");
        // Subtract Player's Life
        var shipMov = FindObjectOfType<ShipMovement>();
        shipMov.currentLives--;

        if (FindObjectOfType<ShipMovement>().currentLives > 0) return;
        //If Game is Over
        shipMov.OnDestroyed();
        GameManager.collided = true;
    }
}
