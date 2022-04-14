using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    /** How many bullets the enemy can shoot per second */
    public float fireRate;
    /** maximum angle of innacuracy */
    public float maxAngle;

    public GameObject bullet;

    private float nextTimeToFire = 0;

    public float projectileSpeed = 15;

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
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float distanceFromPlayer = (playerPos - this.transform.position).magnitude;
        float projectileAirTime = distanceFromPlayer / projectileSpeed;
        if (projectileAirTime > 1f)
        {
            projectileAirTime = 1f;
        }
        Vector3 predictPlayerPos = playerPos + GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity * projectileAirTime;
        Vector3 direction = (predictPlayerPos - this.transform.position).normalized;
        GameObject shotBullet = Instantiate(bullet, this.transform.position, this.transform.rotation);
        /** ignore collisions between the wasps (layer 9) and their bullets (layer 10) */
        Physics.IgnoreLayerCollision(9, 10, true);
        shotBullet.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }
}
