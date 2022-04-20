using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] int generatorHealth = 3;

    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (generatorHealth > 0)
            {
                generatorHealth--;
            }
            else
            {
                shield.SetActive(false);
            }
        }
    }
}
