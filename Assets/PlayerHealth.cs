using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    //public float health;
    [SerializeField] private Shield shield;
    public float maxShieldHealth = 10f;
    public float shieldHealth;

    [SerializeField] private Collider shipCollider;


    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float shieldRegenWaitTime = 1.5f;
    private float invincibilityTimer = 0f;
    private float shieldRegenTimer = 0f;
    private bool regenShield;
    private bool isInvincible;
    [SerializeField] private float regenRate = .37f;

    public bool isRolling;
    
    // Start is called before the first frame update
    void Start()
    {
        shieldHealth = maxShieldHealth;
        shipCollider.enabled = false;
        invincibilityTimer = 0f;
        shieldRegenTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (shieldHealth <= 0f)
        //{
        //    shield.KillShield();
        //}

        if (shieldHealth < maxShieldHealth)
        {
            shieldRegenTimer += Time.deltaTime;
            if (shieldRegenTimer > shieldRegenWaitTime)
                regenShield = true;

            if (regenShield)
                shieldHealth += 1 * regenRate * Time.deltaTime;

            
        }
        else if (regenShield && (shieldHealth >= maxShieldHealth)) {
            regenShield = false;
            shieldRegenTimer = 0;
        }

        if (shieldHealth < 0f) {
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
            if (invincibilityTimer > invincibilityTime) { 
                isInvincible = false;
                invincibilityTimer = 0;
            }
        }

    }

    public bool Damage(float damage) {
        if (isInvincible || isRolling)
            return true;

        if (shieldHealth <= 0f) {
            return false;
        }
        else {
            shieldHealth -= damage;
        }

        isInvincible = true;
        regenShield = false;
        shieldRegenTimer = 0;

        return true;
    }
}
