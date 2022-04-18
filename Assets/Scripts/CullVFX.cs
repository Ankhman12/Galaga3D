using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CullVFX : MonoBehaviour
{
    VisualEffect vfx;

    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vfx.aliveParticleCount == 0) {
            Destroy(this.gameObject, 2f);
        }
    }
}
