using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    public int waspsCount;
    [SerializeField]
    private GameObject waspPrefab;
    [SerializeField]
    public int beetleCount;
    [SerializeField]
    private GameObject beetlePrefab;
    [SerializeField]
    public int scorpionCount;
    [SerializeField]
    private GameObject scorpionPrefab;

    [Header("Target")]
    [SerializeField]
    private GameObject target;

    [Header("Spawner Variables")]
    [SerializeField]
    private float boidSimulationArea = 50f;

    [Header("Flocking Controls")]
    [SerializeField, Range(0f, 100f)]
    private float separationWeight;
    [SerializeField, Range(0f, 100f)]
    private float alignmentWeight;
    [SerializeField, Range(0f, 100f)]
    private float cohesionWeight;
    [SerializeField, Range(0f, 10f)]
    private float targetAggressionWeight;

    [Header("Default Flocking Variables")]
    // Separation
    [SerializeField] private float noClumpingRadius = 100f;
    // Alignment and Cohesion
    [SerializeField] private float localAreaRadius = 10f;
    // Speed
    [SerializeField] private float speed = 10f;
    [SerializeField] private float steeringSpeed = 100f;

    [SerializeField]
    private List<BoidController> wasps;
    [SerializeField]
    private List<BoidController> beetles;
    [SerializeField]
    private List<BoidController> scorpions;
    private bool removing = false;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        wasps = new List<BoidController>();
        beetles = new List<BoidController>();
        scorpions = new List<BoidController>();

        if (waspPrefab != null)
        {
            for (var i = 0; i < waspsCount; i++)
            {
                SpawnBoid(waspPrefab, 0, wasps);
            }
        }
        if (beetlePrefab != null)
        {
            for (var i = 0; i < beetleCount; i++)
            {
                SpawnBoid(beetlePrefab, 0, beetles);
            }
        }
        if (scorpionPrefab != null)
        {
            for (var i = 0; i < scorpionCount; i++)
            {
                SpawnBoid(scorpionPrefab, 0, scorpions);
            }
        }
    }

    private void Update()
    {
        if (!removing)
        {
            foreach (var boid in wasps)
            {
                if (boid != null && boid.GetComponent<BoidController>() != null && boid.gameObject.activeInHierarchy)
                {
                    boid.GetComponent<BoidController>().SimulateMovement(wasps, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight, targetAggressionWeight);

                    var boidPos = boid.transform.position;

                    if (boidPos.x > boidSimulationArea)
                        boidPos.x -= boidSimulationArea * 2;
                    else if (boidPos.x < -boidSimulationArea)
                        boidPos.x += boidSimulationArea * 2;

                    if (boidPos.y > boidSimulationArea)
                        boidPos.y -= boidSimulationArea * 2;
                    else if (boidPos.y < -boidSimulationArea)
                        boidPos.y += boidSimulationArea * 2;

                    if (boidPos.z > boidSimulationArea)
                        boidPos.z -= boidSimulationArea * 2;
                    else if (boidPos.z < -boidSimulationArea)
                        boidPos.z += boidSimulationArea * 2;

                    boid.transform.position = boidPos;
                }
                else
                {
                    wasps.Remove(boid);
                    waspsCount--;
                }

            }

            foreach (var boid in beetles)
            {
                if (boid != null && boid.GetComponent<BoidController>() != null && boid.gameObject.activeInHierarchy)
                {
                    boid.GetComponent<BoidController>().SimulateMovement(beetles, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight, targetAggressionWeight);

                    var boidPos = boid.transform.position;

                    if (boidPos.x > boidSimulationArea)
                        boidPos.x -= boidSimulationArea * 2;
                    else if (boidPos.x < -boidSimulationArea)
                        boidPos.x += boidSimulationArea * 2;

                    if (boidPos.y > boidSimulationArea)
                        boidPos.y -= boidSimulationArea * 2;
                    else if (boidPos.y < -boidSimulationArea)
                        boidPos.y += boidSimulationArea * 2;

                    if (boidPos.z > boidSimulationArea)
                        boidPos.z -= boidSimulationArea * 2;
                    else if (boidPos.z < -boidSimulationArea)
                        boidPos.z += boidSimulationArea * 2;

                    boid.transform.position = boidPos;
                }
                else
                {
                    beetles.Remove(boid);
                    beetleCount--;
                }

            }

            foreach (var boid in scorpions)
            {
                boid.GetComponent<BoidController>().SimulateMovement(scorpions, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight, targetAggressionWeight);

                var boidPos = boid.transform.position;

                if (boidPos.x > boidSimulationArea)
                    boidPos.x -= boidSimulationArea * 2;
                else if (boidPos.x < -boidSimulationArea)
                    boidPos.x += boidSimulationArea * 2;

                if (boidPos.y > boidSimulationArea)
                    boidPos.y -= boidSimulationArea * 2;
                else if (boidPos.y < -boidSimulationArea)
                    boidPos.y += boidSimulationArea * 2;

                if (boidPos.z > boidSimulationArea)
                    boidPos.z -= boidSimulationArea * 2;
                else if (boidPos.z < -boidSimulationArea)
                    boidPos.z += boidSimulationArea * 2;

                boid.transform.position = boidPos;
            }
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex, List<BoidController> list)
    {
        var boidInstance = Instantiate(prefab, transform);
        boidInstance.GetComponent<BoidController>().SetFlockingVariables(noClumpingRadius, localAreaRadius, speed, steeringSpeed, target);
        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        boidInstance.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        var boidController = boidInstance.GetComponent<BoidController>();
        boidController.SwarmIndex = swarmIndex;

        list.Add(boidController);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(boidSimulationArea * 2, boidSimulationArea * 2, boidSimulationArea * 2));
    }

    public void ClearEntities()
    {
        for (int i = 0; i < waspsCount; i++)
        {
            Destroy(wasps[i]);
        }
        for (int i = 0; i < beetleCount; i++)
        {
            Destroy(beetles[i]);
        }
        for (int i = 0; i < scorpionCount; i++)
        {
            Destroy(scorpions[i]);
        }
    }

    public void DisableShooting()
    {
        for (int i = 0; i < waspsCount; i++)
        {
            wasps[i].GetComponent<EnemyShooting>().enabled = false;
        }
        for (int i = 0; i < beetleCount; i++)
        {
            beetles[i].GetComponent<EnemyShooting>().enabled = false;
        }
        for (int i = 0; i < scorpionCount; i++)
        {
            scorpions[i].GetComponent<EnemyShooting>().enabled = false;
        }
    }

    public void removeBoid(BoidController b)
    {
        removing = true;
        if (wasps.Remove(b))
        {
            waspsCount--;
        }
        if (beetles.Remove(b))
        {
            beetleCount--;
        }
        removing = false;
    }
}
