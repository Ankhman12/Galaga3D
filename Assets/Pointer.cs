using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    [SerializeField] private BoidSpawner boidSpawner;
    [SerializeField] private Vector3 targetPos;
    //[SerializeField] private ShipMovement player;

    private void Start()
    {
        FindEnemySpawner();
    }

    private void FindEnemySpawner()
    {
        GameObject boidSpawnerObj = GameObject.FindGameObjectWithTag("EnemySpawner");
        if (boidSpawnerObj) { 
            boidSpawner = boidSpawnerObj.GetComponent<BoidSpawner>();
        }
        //GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (boidSpawner != null) // && player != null)
        {
            List<BoidController> enemies = boidSpawner.GetEnemies();
            targetPos = Vector3.zero;
            /*//Debug.Log(enemies.Count);
            foreach (BoidController enemy in enemies)
            {
                //Debug.Log(enemy.gameObject.transform.position);
                avgPos += enemy.gameObject.transform.position;
               // Debug.Log(avgPos);
            }*/
            if (enemies.Count != 0)
            {
                targetPos = enemies[0].transform.position;
            }
            Debug.DrawRay(this.transform.position, targetPos - this.transform.position, Color.red, Time.deltaTime);
            //Quaternion rotateAmount = Quaternion.FromToRotation(this.transform.rotation.eulerAngles, Quaternion.LookRotation((avgPos - transform.position).normalized).eulerAngles);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((targetPos - transform.position).normalized), Time.deltaTime * 4f);
            //this.transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(this.transform.forward), Quaternion.Euler(targetPos - this.transform.position), 100f);
            Debug.Log(this.transform.rotation.eulerAngles);
        }
        else {
            FindEnemySpawner();
        }
    }

}
