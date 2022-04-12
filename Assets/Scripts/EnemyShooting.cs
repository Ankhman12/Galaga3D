using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    /** How many bullets the enemy can shoot per second */
    public float fireRate;
    /** maximum angle of innacuracy */
    public float maxAngle;

    public GameObject bullet;

    private float nextTimeToFire = 0;

    public float projectileSpeed = 10;

    private void Update()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position).normalized;
        GameObject shotBullet = Instantiate(bullet, this.transform.position, this.transform.rotation);
        /** ignore collisions between the wasps (layer 9) and their bullets (layer 10) */
        Physics.IgnoreLayerCollision(9, 10, true);
        shotBullet.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }
}
