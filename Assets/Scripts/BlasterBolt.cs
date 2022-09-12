using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlasterBolt : MonoBehaviour
{

    Rigidbody rb;
    float timer = 0f;

    public float boltSpeed;
    public float lifeTime;
    public float damage;
    [SerializeField]
    List<GameObject> hitFX;

    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(this.transform.position + (transform.forward * boltSpeed));
        timer += Time.fixedDeltaTime;
        if (timer > lifeTime)  
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid a = collision.collider.GetComponentInParent<Asteroid>();
            a.TakeDamage(damage);
            Debug.Log("Applying damage to: " + a.gameObject.name);
            foreach (GameObject v in hitFX)
            {
                
                Instantiate(v, collision.transform.position, collision.transform.rotation);
            }
            Destroy(this.gameObject);
            //StartCoroutine(Kill());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStats a = collision.collider.GetComponentInParent<EnemyStats>();
            if (a != null)
            {
                a.TakeDamage(damage);
                Debug.Log("Applying damage to: " + a.gameObject.name);
            }
            foreach (GameObject v in hitFX)
            {
                
                Instantiate(v, collision.GetContact(0).point, collision.transform.rotation);
            }
            Destroy(this.gameObject);
            //StartCoroutine(Kill());
        }
        /*if (this.gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Player"))
        {
            Shield s = collision.collider.GetComponent<Shield>();
            //a.Hit(damage);
            //Debug.Log("Hitting shield of: " + a.transform.parent.gameObject.name);
            foreach (GameObject v in hitFX)
            {

                Instantiate(v, collision.GetContact(0).point, collision.transform.rotation);
            }
            Destroy(this.gameObject);
            //StartCoroutine(Kill());
        }*/






    }
    }
