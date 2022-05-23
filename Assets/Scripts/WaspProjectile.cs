using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspProjectile : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    private float timer = 0;

    [SerializeField] private float damage = 1f;

    [SerializeField]
    List<GameObject> hitFX;

    private void FixedUpdate()
    {
        if (timer >= despawnTime)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Collided with Player");
        // Subtract Player's Life
        var shipMov = FindObjectOfType<ShipMovement>();
        shipMov.currentLives--;


        if (FindObjectOfType<ShipMovement>().currentLives <= 0)
        {
            //If Game is Over
            shipMov.OnDestroyed();
            GameManager.collided = true;
        } else
        {
            GameManager.Instance.hurtPlayer();
            Destroy(this.gameObject);
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            // Subtract Player's Life
            var shipMov = FindObjectOfType<ShipMovement>();
            shipMov.Damage(damage);


            if (FindObjectOfType<ShipMovement>().currentLives <= 0)
            {
                //If Game is Over
                shipMov.OnDestroyed();
                GameManager.collided = true;
            }
            else
            {

                
                //Debug.Log("Hitting shield of: " + a.transform.parent.gameObject.name);
                foreach (GameObject v in hitFX)
                {
                    Instantiate(v, collision.GetContact(0).point, collision.transform.rotation);
                }
                //Destroy(this.gameObject);
                //StartCoroutine(Kill());
                Destroy(this.gameObject);
            }

        }
    }
}
