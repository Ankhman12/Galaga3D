using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject waspPrefab;
    [SerializeField]
    private GameObject beetlePrefab;
    [SerializeField]
    private GameObject scorpionPrefab;

    [Header("Target")] 
    [SerializeField] 
    private GameObject target;
    
    [Header("Spawner Variables")]
    [SerializeField]
    private int spawnBoids = 100;
    [SerializeField]
    private float boidSimulationArea = 50f;

    [Header("Flocking Controls")] 
    [SerializeField, Range(0f, 10f)]
    private float separationWeight;
    [SerializeField, Range(0f, 10f)]
    private float alignmentWeight;
    [SerializeField, Range(0f, 10f)]
    private float cohesionWeight;
    
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

    private void Start()
    {
        wasps = new List<BoidController>();
        beetles = new List<BoidController>();
        scorpions = new List<BoidController>();

        for (var i = 0; i < spawnBoids/3; i++)
        {
            SpawnBoid(waspPrefab, 0, wasps);
        }
        for (var i = 0; i < spawnBoids/3; i++)
        {
            SpawnBoid(beetlePrefab, 1, beetles);
        }
        for (var i = 0; i < spawnBoids/3; i++)
        {
            SpawnBoid(scorpionPrefab, 2, scorpions);
        }
    }

    private void Update()
    {
        foreach (var boid in wasps)
        {
            boid.GetComponent<BoidController>().SimulateMovement(wasps, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight);

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
        
        foreach (var boid in beetles)
        {
            boid.GetComponent<BoidController>().SimulateMovement(beetles, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight);

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
        
        foreach (var boid in scorpions)
        {
            boid.GetComponent<BoidController>().SimulateMovement(scorpions, Time.deltaTime, separationWeight, alignmentWeight, cohesionWeight);

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
}
