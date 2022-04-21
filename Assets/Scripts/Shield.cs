using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Material shieldMAT;
    private bool isHit = false;
    [SerializeField] private float hitFlashTime = 0.5f;
    private float hitTimer = 0f;
    bool isShrinking = false;
    bool isKilled = false;

    [SerializeField] float growShrinkScale;
    float startScale;
    float targetScale;

    private void Start()
    {
        shieldMAT.SetInt("IsHit_", 0);
        isHit = false;
        isShrinking = false;
        isKilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit) {

            //Debug.Log("yololo");
            if (isShrinking)
            {
                //Debug.Log(this.transform.localScale.x);
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, (hitTimer / (hitFlashTime / 2)));
                this.gameObject.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
                if (this.transform.localScale.x == targetScale)
                {
                    SetGrowing();
                }
            }
            else
            {
                //Debug.Log("cuts");
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, ((hitTimer - (hitFlashTime / 2)) / (hitFlashTime / 2)));
                this.gameObject.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
            }

            if (isKilled)
            {
                float newScaleVal = Mathf.SmoothStep(startScale, targetScale, (hitTimer / (hitFlashTime / 2)));
                this.transform.localScale = new Vector3(newScaleVal, newScaleVal, newScaleVal);
            }

            hitTimer += Time.deltaTime;
            if (hitTimer > hitFlashTime)
            {
                shieldMAT.SetInt("IsHit_", 0);
                isHit = false;
                hitTimer = 0;
            }
            
        }
        /*if (isShrinking)
        {
            float newScaleVal = Mathf.Lerp(startScale, targetScale, hitFlashTime / 2);
            this.transform.localScale.Set(newScaleVal, newScaleVal, newScaleVal);
            if (this.transform.localScale.x == targetScale)
            {
                SetGrowing();
            }
        }
        else
        {
            float newScaleVal = Mathf.Lerp(startScale, targetScale, hitFlashTime / 2);
            this.transform.localScale.Set(newScaleVal, newScaleVal, newScaleVal);
        }*/
        
    }

    private void SetShrinking() 
    {
        isShrinking = true;
        startScale = this.gameObject.transform.localScale.x;
        targetScale = this.gameObject.transform.localScale.x - growShrinkScale;
    }

    private void SetGrowing()
    {
        isShrinking = false;
        startScale = this.gameObject.transform.localScale.x;
        targetScale = this.gameObject.transform.localScale.x + growShrinkScale;
    }

    public void KillShield() {
        isKilled = true;
        startScale = this.gameObject.transform.localScale.x;
        targetScale = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((this.gameObject.layer == 13 && collision.gameObject.CompareTag("Enemy")) || (this.gameObject.layer == 12))
        {

            shieldMAT.SetInt("IsHit_", 1);
            isHit = true;
            //isShrinking = true;
            SetShrinking();
        }
    }

}
