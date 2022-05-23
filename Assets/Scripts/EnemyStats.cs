using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float health = 100f;



    // Reference to Player
    [SerializeField] private ShipMovement shipMov;
    
    public VisualEffect breakFX;
    public AudioSource breakSFX;

    private void Awake()
    {
        shipMov = FindObjectOfType<ShipMovement>();
        health = maxHealth;
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
            shipMov.Damage(1f);

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
        BoidSpawner b = this.gameObject.GetComponentInParent<BoidSpawner>();
        if (b != null)
        {
            b.removeBoid(this.gameObject.GetComponent<BoidController>());
        }
        Destroy(this.gameObject);
    }
    
    private void DestroySequence()
    {
        //Instantiate Destruction Particles
         Instantiate(breakFX.gameObject, transform.position, transform.rotation);
        //Play Explosion SFX
        if (breakSFX != null)
        {
            Instantiate(breakSFX, transform.position, transform.rotation);

        }
        //breakSFX.Play();
    }

    public float GetCurrentHealth() 
    {
        return this.health;    
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

}