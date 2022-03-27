using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBolt : MonoBehaviour
{

    Rigidbody rb;
    float timer = 0f;

    public float boltSpeed;
    public float lifeTime;
    public float damage;
    [SerializeField]
    List<ParticleSystem> hitFX;

    
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
        Asteroid a = collision.collider.GetComponentInParent<Asteroid>();
        if (a) {
            a.TakeDamage(damage);
            Debug.Log("Applying damage to: " + a.gameObject.name);
            Destroy(this.gameObject);
        }
        
    }
}
