using System;
using UnityEngine;
using TMPro;

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
    private float initialShootingTimer = 5f;
    private float timer = 0;
    [SerializeField] enum EnemyType
    {
        Beetle,
        Wasp
    }
    [SerializeField] private EnemyType type;
    enum BeetleState
    {
        Idle,
        Charging,
        Fire,
        Sleep
    }
    private BeetleState beetleState = BeetleState.Idle;
    private float beetleTimer = 0;
    private float beetleChargeTime = 1.5f;
    private GameObject chargingBeetleBullet;
    private float beetleSleepTime = 3f;
    [SerializeField] ParticleSystem beetleParticles;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (timer < initialShootingTimer)
        {
            timer += Time.deltaTime;
            return;
        }
        if (type == EnemyType.Wasp)
        {
            if (Time.time >= nextTimeToFire && startShooting)
            {
                // Check Time
                nextTimeToFire = Time.time + 1f / fireRate;
                // Is Player in Raycast Cone
                Debug.Log("Shooting at Player.");
                Shoot();

            }
        } else if (type == EnemyType.Beetle)
        {
            switch(beetleState)
            {
                case BeetleState.Idle:
                    //Debug.Log("idle");
                    if (startShooting)
                    {
                        beetleParticles.Play();
                        beetleTimer = 0;
                        beetleState = BeetleState.Charging;
                        chargingBeetleBullet = Instantiate(bullet, beetleParticles.gameObject.transform.position, transform.rotation);
                        chargingBeetleBullet.transform.parent = this.gameObject.transform;
                        chargingBeetleBullet.gameObject.transform.localScale = new Vector3(.1f, .1f, .1f);
                    }
                    break;
                case BeetleState.Charging:
                    //Debug.Log("charging");
                    if (beetleTimer >= beetleChargeTime)
                    {
                        beetleState = BeetleState.Fire;
                        beetleParticles.Stop();
                    } else
                    {
                        float deltat = Time.deltaTime;
                        if (chargingBeetleBullet.gameObject.transform.localScale.x < 12)
                        {
                            float newScale = chargingBeetleBullet.gameObject.transform.localScale.x + .05f;
                            chargingBeetleBullet.gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
                            //Set particle system scale, for some reason it doesn't scale with the parent (its the first child / index 0)
                            chargingBeetleBullet.gameObject.transform.GetChild(0).localScale = new Vector3(newScale, newScale, newScale);
                        }
                        beetleTimer += deltat;
                    }
                    break;
                case BeetleState.Fire:
                    //Debug.Log("Fire");
                    chargingBeetleBullet.transform.parent = null;
                    Shoot();
                    beetleTimer = 0;
                    beetleState = BeetleState.Sleep;
                    break;
                case BeetleState.Sleep:
                    //Debug.Log("sleepy");
                    if (beetleTimer >= beetleSleepTime)
                    {
                        beetleState = BeetleState.Idle;
                    } else
                    {
                        beetleTimer += Time.deltaTime;
                    }
                    break;
                default:
                    break;
            }

        }
    }

    public void Shoot()
    {
        if (target != null) targetPos = target.transform.position;

       // GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "1";
        var distanceFromPlayer = (targetPos - transform.position).magnitude;
       // GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "2";
        var projectileAirTime = distanceFromPlayer / projectileSpeed;
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "3";
        if (projectileAirTime > 1f) projectileAirTime = 1f;
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "4";
        if (type == EnemyType.Beetle && projectileAirTime > 0.5f) {
            projectileAirTime = 0.5f;
        }
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "5";
        var predictPlayerPos = targetPos;
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "6";
        if (target != null)
        {
            GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "6.25";
            if (target.GetComponent<ShipMovement>() == null)
            {
                GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "null rigid";
            }
            predictPlayerPos = targetPos + target.GetComponentInChildren<Rigidbody>().velocity * projectileAirTime;
        }
            
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "6.5";
        var direction = (predictPlayerPos - transform.position).normalized;
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "7";
        Quaternion projectileRotation = Quaternion.LookRotation(direction, transform.up);
        //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "8";
        if (type == EnemyType.Wasp)
        {
            var shotBullet = Instantiate(bullet, transform.position, projectileRotation);
            //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "9";
            //GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "9";
            /** ignore collisions between the wasps (layer 6) and their bullets (layer 10) */
            Physics.IgnoreLayerCollision(6, 10, true);
            shotBullet.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        } else if (type == EnemyType.Beetle)
        {
            Physics.IgnoreLayerCollision(6, 10, true);
            chargingBeetleBullet.GetComponent<Rigidbody>().velocity = direction * projectileSpeed * 3f;
        }
        
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