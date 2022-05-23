using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{

    [Header("Ship General Setting")]
    [SerializeField]
    public int currentLives = 3;
    [SerializeField]
    public PlayerHealth playerHealth;
    [SerializeField]
    private float mouseSensitivity = 1f;

    [Header("Ship Third Person Movement Setting")]
    [SerializeField]
    private float tpYawTorque = 500f;
    [SerializeField]
    private float tpPitchTorque = 1000f;
    [SerializeField]
    private float tpRollTorque = 1000f;
    [SerializeField]
    private float tpThrust = 100f;
    [SerializeField]
    private float tpUpThrust = 50f;
    [SerializeField]
    private float tpStrafeThrust = 50f;

    [Header("Ship First Person Movement Setting")]
    [SerializeField]
    private float fpRollTorque = 1000f;
    [SerializeField]
    private float fpThrust = 100f;
    [SerializeField]
    private float fpUpThrust = 50f;
    [SerializeField]
    private float fpStrafeThrust = 50f;

    [Header("Boost Setting")]
    [SerializeField]
    public float maxBoostAmount = 2f;
    [SerializeField]
    private float boostDegenerationRate = 0.25f;
    [SerializeField]
    private float boostRechargeRate = 0.5f;
    [SerializeField]
    private float boostMultiplier = 5f;
    public bool boosting = false;
    public float currentBoostAmount;

    [SerializeField] public CinemachineVirtualCamera shipFirstPersonCam;
    [SerializeField] public CinemachineVirtualCamera shipThirdPersonCam;

    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.111f;
    float glide, verticalGlide, horizontalGlide = 0f;

    Rigidbody rb;

    // Input Values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;

    public bool isThirdPerson = true;

    //FX variables
    [SerializeField]
    GameObject boostFX;
    [SerializeField]
    VisualEffect destructFX;
    [SerializeField]
    AudioSource destructSFX;
    [SerializeField] 
    GameObject thrusterVFX; 
    [SerializeField]
    AudioSource thrusterSFX;
    bool thrusting = false;
    bool upDowning = false;
    bool strafing = false;
    [SerializeField]
    AudioSource boostSFX;
    [SerializeField]
    AudioSource bumpSFX;

    void Start()
    {
        if (shipFirstPersonCam != null)
        {
            CinemachineCameraSwitcher.Register(shipFirstPersonCam);
        }
        if (shipThirdPersonCam != null)
        {
            CinemachineCameraSwitcher.Register(shipThirdPersonCam);
        }
        CinemachineCameraSwitcher.SwitchCamera(shipThirdPersonCam);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentBoostAmount = maxBoostAmount;
        currentLives = 3;
        mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.5f);
    }

    void FixedUpdate()
    {
        HandleBoosting();
        HandleMovement();
        HandleFX();
    }

    void HandleFX() 
    {
        if (boosting)
        {
            if (!boostSFX.isPlaying)
            {
                boostSFX.Play();
            }
            /*foreach (ParticleSystem p in boostFX)
            {
                p.Play();
            }*/
            boostFX.SetActive(true);
            thrusterVFX.SetActive(false);
        }
        else if (thrust1D > 0) {
            thrusting = true;
        }
        else if (thrust1D < 0) {
            thrusting = true;
        } 
        else
        {
            thrusting = false;
            boostSFX.Stop();
            boostFX.SetActive(false);
            thrusterVFX.SetActive(false);
        }

        if (upDown1D > 0)
        {
            upDowning = true;

        }
        else if (upDown1D < 0)
        {
            upDowning = true;
        }
        else
        {
            upDowning = false;
        }

        if (strafe1D > 0)
        {
            strafing = true;

        }
        else if (strafe1D < 0)
        { 
            strafing = true;
        }
        else {
            strafing = false;
        }

        if (thrusting || upDowning || strafing)
        {
            if (thrusterSFX != null && !thrusterSFX.isPlaying)
            {
                thrusterSFX.Play();
                //Debug.Log("Yah");
            }
            thrusterVFX.SetActive(true);

        }
        if (!thrusting && !upDowning && !strafing)
        {
            if (thrusterSFX.isPlaying) {
                thrusterSFX.Stop();
                //Debug.Log("Yeety");
            }

            thrusterVFX.SetActive(false);
        }
        
    }

    void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDegenerationRate;
            if (currentBoostAmount <= 0f)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }

    void HandleMovement()
    {
        if (isThirdPerson)
        {
            // Roll
            rb.AddRelativeTorque(Vector3.back * roll1D * tpRollTorque * Time.fixedDeltaTime);
            // Pitch
            rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * tpPitchTorque * Time.fixedDeltaTime);
            // Yaw
            rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * tpYawTorque * Time.fixedDeltaTime);

            // Thrust
            if (boosting)
            {
                float currentThrust;
                currentThrust = tpThrust * boostMultiplier;
                rb.AddForce(transform.forward * currentThrust * Time.fixedDeltaTime);
                glide = thrust1D * currentThrust;
            }
            else if (thrust1D > 0.1f || thrust1D < -0.1f)
            {
                float currentThrust;
                currentThrust = tpThrust;
                rb.AddForce(transform.forward * thrust1D * currentThrust * Time.fixedDeltaTime);
                glide = thrust1D * currentThrust;
            }
            else
            {
                rb.AddForce(transform.forward * glide * Time.fixedDeltaTime);
                glide *= thrustGlideReduction;
            }

            // Up/Down
            if (upDown1D > 0.1f || upDown1D < -0.1f)
            {
                rb.AddRelativeForce(Vector3.up * upDown1D * tpUpThrust * Time.fixedDeltaTime);
                verticalGlide = upDown1D * tpUpThrust;
            }
            else
            {
                rb.AddRelativeForce(Vector3.up * verticalGlide * Time.fixedDeltaTime);
                verticalGlide *= upDownGlideReduction;
            }
            // Strafing
            if (strafe1D > 0.1f || strafe1D < -0.1f)
            {
                rb.AddRelativeForce(Vector3.right * strafe1D * tpStrafeThrust * Time.fixedDeltaTime);
                horizontalGlide = strafe1D * tpStrafeThrust;
            }
            else
            {
                rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.fixedDeltaTime);
                horizontalGlide *= leftRightGlideReduction;
            }
        }
        else
        {
            // Roll
            rb.AddTorque(-shipFirstPersonCam.transform.forward * roll1D * fpRollTorque * Time.fixedDeltaTime);
            // Pitch
            rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * tpPitchTorque * Time.fixedDeltaTime);
            // Yaw
            rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * tpYawTorque * Time.fixedDeltaTime);

            // Thrust
            if (thrust1D > 0.1f || thrust1D < -0.1f)
            {
                float currentThrust;

                if (boosting)
                {
                    currentThrust = fpThrust * boostMultiplier;
                }
                else
                {
                    currentThrust = fpThrust;
                }
                rb.AddForce(shipFirstPersonCam.transform.forward * thrust1D * currentThrust * Time.fixedDeltaTime);
                glide = thrust1D * currentThrust;
            }
            else
            {
                rb.AddForce(shipFirstPersonCam.transform.forward * glide * Time.fixedDeltaTime);
                glide *= thrustGlideReduction;
            }

            // Up/Down
            if (upDown1D > 0.1f || upDown1D < -0.1f)
            {
                rb.AddForce(transform.up * upDown1D * fpUpThrust * Time.fixedDeltaTime);
                verticalGlide = upDown1D * fpUpThrust;
            }
            else
            {
                rb.AddForce(transform.up * verticalGlide * Time.fixedDeltaTime);
                verticalGlide *= upDownGlideReduction;
            }
            // Strafing
            if (strafe1D > 0.1f || strafe1D < -0.1f)
            {
                rb.AddForce(shipFirstPersonCam.transform.right * strafe1D * fpStrafeThrust * Time.fixedDeltaTime);
                horizontalGlide = strafe1D * fpStrafeThrust;
            }
            else
            {
                rb.AddForce(shipFirstPersonCam.transform.right * horizontalGlide * Time.fixedDeltaTime);
                horizontalGlide *= leftRightGlideReduction;
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

    public void OnDestroyed() 
    {
        destructSFX.Play();
        Instantiate(destructFX.gameObject, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Damage(float damage) {
        if (!playerHealth.Damage(damage)) {
            currentLives--;
        }
    }

    #region Input Methods
    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
        
    }
    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
        
    }
    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
        
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
        
    }
    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>() * mouseSensitivity;
        
    }
    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
        
    }

    public void OnToggleCamera(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            
            if (CinemachineCameraSwitcher.isActiveCamera(shipThirdPersonCam))
            {
                Debug.Log("Switching Camera to First Person");
                CinemachineCameraSwitcher.SwitchCamera(shipFirstPersonCam);
                isThirdPerson = false;
            }
            else if (CinemachineCameraSwitcher.isActiveCamera(shipFirstPersonCam))
            {
                Debug.Log("Switching Camera to Third Person");
                CinemachineCameraSwitcher.SwitchCamera(shipThirdPersonCam);
                isThirdPerson = true;
            }

        }
        Debug.Log(thrust1D);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.gameIsPaused = !GameManager.gameIsPaused;
            GameManager.Instance.PauseGame();
        }

    }
    #endregion
}
