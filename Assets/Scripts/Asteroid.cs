using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    Rigidbody rb;

    public float initialForce = 10f;
    public float initialTorque = 10f;

    public float health;
    public int size;
    public List<ParticleSystem> breakFX;
    public AudioSource breakSFX;
    private int id;
    
    public Asteroid(int initSize)
    {
        size = initSize;
        id = GameManager.currentID;
        GameManager.currentID++;
        //LARGE
        if (size == 2)
        {
            health = 5;
        }
        //MEDIUM
        else if (size == 1)
        {
            health = 2;
        }
        //SMALL
        else
        {
            health = 1;
        }
    }
    
    public Asteroid()
    {
        id = GameManager.currentID;
        GameManager.currentID++;
        //LARGE
        if (size == 2)
        {
            health = 5;
        }
        //MEDIUM
        else if (size == 1)
        {
            health = 2;
        }
        //SMALL
        else
        {
            health = 1;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        //rb.isKinematic = true;

        int startDirA = Random.Range(0, 5);
        int startDirB = Random.Range(0, 5);
        float modA = Random.Range(0.5f, 1);
        float modB = Random.Range(0.5f, 1);
        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.up, Vector3.down, Vector3.left, Vector3.back };
        Vector3 newDir = ((modA * directions[startDirA]) + (modB * directions[startDirB])).normalized;

        rb.AddForce(newDir * Random.Range(initialForce / 2f, initialForce));
        rb.AddTorque(directions[startDirA] * Random.Range(initialTorque / 2f, initialTorque));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.endOfGame)
        {
            CollisionChecker();
            bool hit = false;
            // bool hit = HitChecker();
            if (hit)
            {
                health -= 1;
            }
            if (health <= 0)
            {
                Destroyed();
            }
        }
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
    
    void Destroyed()
    {

        OnDestroyed();
        // Split each asteroid into two other asteroids if it is big enough
        if (size == 2)
        {
            GameManager.points += 100;
            GameManager.asteroidSpawner.SpawnAsteroid(this.transform.position, 1);
            
            Destroy(this.gameObject);
            //GameManager.asteroidArray[id] = new Asteroid(1);
            //GameManager.asteroidArray[GameManager.currentID] = new Asteroid(1);
        }
        else if (size == 1)
        {
            GameManager.points += 250;
            GameManager.asteroidSpawner.SpawnAsteroid(this.transform.position, 0);
            //OnDestroyed();
            Destroy(this.gameObject);
            //GameManager.asteroidArray[id] = new Asteroid(0);
            //GameManager.asteroidArray[GameManager.currentID] = new Asteroid(0);
        }
        else
        {
            GameManager.points += 500;
            //OnDestroyed();
            Destroy(this.gameObject);
            //GameManager.asteroidArray[id] = null;
        }
        //OnDestroyed();
    }
    
    void CollisionChecker()
    {
        //if distance(spaceship, asteroid) < ##
        //{
        //  Globals.collided = true;  
        //
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            Debug.Log("Collided with Player");
            // Subtract Player's Life
            ShipMovement shipMov = FindObjectOfType<ShipMovement>();
            shipMov.currentLives--;

            if (FindObjectOfType<ShipMovement>().currentLives <= 0)
            {
                //If Game is Over
                shipMov.OnDestroyed();
                GameManager.collided = true;
            }


              
        }
    }

    private void OnDestroyed()
    {
        //Instantiate Destruction Particles
        foreach (ParticleSystem p in breakFX)
        {
            Instantiate(p.gameObject, transform.position, transform.rotation);
        }
        //Play Explosion SFX
        Instantiate(breakSFX, transform.position, transform.rotation);
        //breakSFX.Play();
    }

}
