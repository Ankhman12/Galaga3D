using System;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    /** How many bullets the enemy can shoot per second */
    [SerializeField] private float fireRate;

    /** maximum angle of innacuracy */
    [SerializeField] private float maxAngle;

    [SerializeField] public GameObject bullet;
    private float nextTimeToFire = 0;
    [SerializeField] private float projectileSpeed = 15;
    private Vector3 targetPos;
    private GameObject target;
    private bool startShooting = false;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Time.time >= nextTimeToFire && startShooting)
        {
            // Check Time
            nextTimeToFire = Time.time + 1f / fireRate;
            // Is Player in Raycast Cone
            Debug.Log("Shooting at Player.");
            Shoot();
        }
    }

    public void Shoot()
    {
        if (target != null) targetPos = target.transform.position;
        var distanceFromPlayer = (targetPos - transform.position).magnitude;
        var projectileAirTime = distanceFromPlayer / projectileSpeed;
        if (projectileAirTime > 1f) projectileAirTime = 1f;
        var predictPlayerPos = targetPos;
        if (target != null)
            predictPlayerPos = targetPos + target.GetComponent<Rigidbody>().velocity * projectileAirTime;
        var direction = (predictPlayerPos - transform.position).normalized;
        Quaternion projectileRotation = Quaternion.LookRotation(direction, transform.up);
        var shotBullet = Instantiate(bullet, transform.position, projectileRotation);
        /** ignore collisions between the wasps (layer 9) and their bullets (layer 10) */
        Physics.IgnoreLayerCollision(9, 10, true);
        shotBullet.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        startShooting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        startShooting = false;
    }
}