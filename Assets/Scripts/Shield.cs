using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject ShieldObject;
    
    [SerializeField] private Material shieldMAT;
    private bool isHit = false;
    [SerializeField] private float hitFlashTime = 0.5f;
    private float hitTimer = 0f;

    private bool isShrinking;
    private bool isBreaking;
    [SerializeField] private float growShrinkScale;
    float startScale;
    float targetScale;

    private void Start()
    {
        shieldMAT.SetInt("IsHit_", 0);
        isHit = false;
        isShrinking = false;
        isBreaking = false;
}

    // Update is called once per frame
    void Update()
    {
        if (isHit) {
            if (isShrinking) {
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, (hitTimer / (hitFlashTime / 2)));
                ShieldObject.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
                if (ShieldObject.transform.localScale.x == targetScale)
                {
                    StartGrowing();
                }
            }
            else {
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, ((hitTimer - (hitFlashTime / 2)) / (hitFlashTime / 2)));
                ShieldObject.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
            }

            if (isBreaking) {
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, (hitTimer / (hitFlashTime / 2)));
                ShieldObject.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
            }
            
            hitTimer += Time.deltaTime;
            if (hitTimer > hitFlashTime)
            {
                shieldMAT.SetInt("IsHit_", 0);
                isHit = false;
                hitTimer = 0;
            }
        }
    }

    private void StartShrinking() {
        isShrinking = true;
        startScale = ShieldObject.transform.localScale.x;
        targetScale = ShieldObject.transform.localScale.x - growShrinkScale;
    }

    private void StartGrowing() {
        isShrinking = false;
        startScale = ShieldObject.transform.localScale.x;
        targetScale = ShieldObject.transform.localScale.x + growShrinkScale;
    }

    public void BreakShield() {
        isBreaking = true;
        startScale = ShieldObject.transform.localScale.x;
        targetScale = 0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 11 || collision.collider.gameObject.layer == 11)
        {
            //Debug.Log("Hit Enemy Shield");
            shieldMAT.SetInt("IsHit_", 1);
            isHit = true;
            StartShrinking();
        }
    }

}
