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
        Asteroid a = collision.collider.GetComponentInParent<Asteroid>();
        bool hit = collision.gameObject.CompareTag("Asteroid");
        if (hit) {
            a.TakeDamage(damage);
            Debug.Log("Applying damage to: " + a.gameObject.name);
            foreach (GameObject v in hitFX)
            {
                
                Instantiate(v, collision.transform.position, collision.transform.rotation);
                //Debug.Log("dkfgh;jhs;fh");
            }
            Destroy(this.gameObject);
            //StartCoroutine(Kill());
        }
        
    }

    //IEnumerator Kill() {
    //   yield return new WaitForSeconds(.1f);
    //    Destroy(this.gameObject);
        //yield return null;
    //}
}
