using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")] 
    [SerializeField] private float health = 100f;

    [SerializeField] private int damage = 1;

    // Reference to Player
    [SerializeField] private ShipMovement shipMov;
    
    public List<ParticleSystem> breakFX;
    public AudioSource breakSFX;

    private void Awake()
    {
        shipMov = FindObjectOfType<ShipMovement>();
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"Taking damage, new health at {health}");

        if (health <= 0)
        {
            Destroyed();
        }
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            // Subtract Player's Life
            shipMov.currentLives--;

            if (shipMov.currentLives <= 0)
            {
                //If Game is Over
                shipMov.OnDestroyed();
                GameManager.collided = true;
            }
        }
    }

    private void Destroyed()
    {
        GameManager.Instance.AddPoints(100);
        DestroySequence();
        Destroy(this.gameObject);
    }
    
    private void DestroySequence()
    {
        //Instantiate Destruction Particles
        foreach (ParticleSystem p in breakFX)
        {
            Instantiate(p.gameObject, transform.position, transform.rotation);
        }
        //Play Explosion SFX
        if (breakSFX != null)
        {
            Instantiate(breakSFX, transform.position, transform.rotation);

        }
        //breakSFX.Play();
    }
    
}