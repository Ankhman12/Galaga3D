using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float maxShield = 7f;
    private int currentHealth;
    private float currentShield;

    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float shieldRegenWaitTime = 3f;
    [SerializeField] private float shieldRegenRate = 0.5f;
    private float invincibilityTimer = 0f;
    private float shieldRegenTimer = 0f;
    private bool isRegenShield = false;
    private bool isInvincible = false;
    private bool isRolling;

    [SerializeField] private Collider shipCollider;

    [SerializeField] private VisualEffect destructVFX;
    [SerializeField] private AudioSource destructSFX;
    [SerializeField] private AudioSource bumpSFX;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        shipCollider.enabled = false;
        invincibilityTimer = 0f;
        shieldRegenTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShield < maxShield)
        {
            shieldRegenTimer += Time.deltaTime;
            if (shieldRegenTimer > shieldRegenWaitTime)
                isRegenShield = true;

            if (isRegenShield)
                currentShield += 1 * shieldRegenRate * Time.deltaTime;


        }
        else if (isRegenShield && (currentShield >= maxShield))
        {
            isRegenShield = false;
            shieldRegenTimer = 0;
        }

        if (currentShield < 0f)
        {
            if (!shipCollider.enabled)
            {
                shipCollider.enabled = true;
            }
        }
        else if (shipCollider.enabled)
        {
            shipCollider.enabled = false;
        }

        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer > invincibilityTime)
            {
                isInvincible = false;
                invincibilityTimer = 0;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Collided with Asteroid");
            //this.enabled = false;
            bumpSFX.Play();
        }
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy");
            //this.enabled = false;
            bumpSFX.Play();
        }
        if (collision.collider.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collided with Projectile");
            //this.enabled = false;
            bumpSFX.Play();
        }

    }

    public float CurrentHealth() 
    {
        return this.currentHealth;
    }

    public float CurrentShield()
    {
        return this.currentShield;
    }

    public void Destruct()
    {
        Instantiate(destructVFX.gameObject, transform.position, transform.rotation);
        destructSFX.Play();
        Destroy(this.gameObject);
    }

    public void setRolling(bool rolling) 
    {
        isRolling = rolling;
    }

    public void Damage(float damage)
    {
        if (isInvincible || isRolling)
            return;

        if (currentShield > 0f)
        {
            currentShield -= damage;
        }
        else {
            currentHealth--;
        }

        isInvincible = true;
    }
}
