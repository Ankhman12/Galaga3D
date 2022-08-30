using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    private bool hasTarget;
    private GameObject target;

    void FixedUpdate()
    {
        if (target == null)
        {
            hasTarget = false;
        }
        Vector3 targetDir = target.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.FromToRotation(transform.rotation.eulerAngles, targetDir), 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            if (!hasTarget)
            {
                target = other.gameObject;
                hasTarget = true;
            }
            else { 
                // add to a buffer for next targets
            }
        }
    }
}
