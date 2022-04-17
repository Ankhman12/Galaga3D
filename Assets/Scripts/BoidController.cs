using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int SwarmIndex { get; set; }
    [Header("Flocking Variables")]
    // Separation
    private float noClumpingRadius = 100f;
    // Alignment and Cohesion
    private float localAreaRadius = 10f;
    // Speed
    private float speed = 10f;
    private float steeringSpeed = 100f;
    [SerializeField]
    Vector3 steering = Vector3.zero;

    [Header("Target")] 
    [SerializeField] 
    private GameObject target;
    [SerializeField] 
    private bool hasTarget;
    
    Vector3 separationDirection = Vector3.zero;
    int separationCount;
    Vector3 alignmentDirection = Vector3.zero;
    int alignmentCount;
    Vector3 cohesionDirection = Vector3.zero;
    int cohesionCount;

    public void SetFlockingVariables(float noClumpingRadius, float localAreaRadius, float speed, float steeringSpeed)
    {
        this.noClumpingRadius = noClumpingRadius;
        this.localAreaRadius = localAreaRadius;
        this.speed = speed;
        this.steeringSpeed = steeringSpeed;
        hasTarget = false;
    }

    public void SetFlockingVariables(float noClumpingRadius, float localAreaRadius, float speed, float steeringSpeed, GameObject target)
    {
        this.noClumpingRadius = noClumpingRadius;
        this.localAreaRadius = localAreaRadius;
        this.speed = speed;
        this.steeringSpeed = steeringSpeed;
        this.target = target;
        hasTarget = true;
    }
    
    public void SimulateMovement(List<BoidController> other, float time)
    {
        //default vars
        steering = Vector3.zero;

        separationDirection = Vector3.zero;
        separationCount = 0;
        alignmentDirection = Vector3.zero;
        alignmentCount = 0;
        cohesionDirection = Vector3.zero;
        cohesionCount = 0;

        var leaderBoid = other[0];
        var leaderAngle = 180f;

        foreach (BoidController boid in other)
        {
            Transform boidTransfrom = boid.transform;
            //skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour
            if (distance < noClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

            //identify local neighbour
            if (distance < localAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boidTransfrom.position - transform.position;
                cohesionCount++;

                //identify leader
                var angle = Vector3.Angle(boidTransfrom.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        //flip
        separationDirection = -separationDirection;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        //get direction to center of mass
        cohesionDirection -= transform.position;

        //weighted rules
        steering += separationDirection.normalized;
        steering += alignmentDirection.normalized;
        steering += cohesionDirection.normalized;

        //local leader
        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized;

        //obstacle avoidance
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, localAreaRadius, LayerMask.GetMask("Default")))
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, speed)) * time;
    }
    public void SimulateMovement(List<BoidController> other, float time, float separationWeight, float alignmentWeight, float cohesionWeight, float targetAggression)
    {
        //default vars
        steering = Vector3.zero;

        separationDirection = Vector3.zero;
        separationCount = 0;
        alignmentDirection = Vector3.zero;
        alignmentCount = 0;
        cohesionDirection = Vector3.zero;
        cohesionCount = 0;

        Vector3 follow = Vector3.zero;
        if (hasTarget && target != null)
        {
            follow = target.transform.position;
        }
        var leaderBoid = other[0];
        var leaderAngle = 180f;

        foreach (BoidController boid in other)
        {
            Transform boidTransfrom = boid.transform;
            //skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour
            if (distance < noClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

            //identify local neighbour
            if (distance < localAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boidTransfrom.position - transform.position;
                cohesionCount++;

                //identify leader
                var angle = Vector3.Angle(boidTransfrom.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        //flip
        separationDirection = -separationDirection;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        //get direction to center of mass
        cohesionDirection -= transform.position;

        //weighted rules
        steering += separationDirection.normalized * separationWeight;
        steering += alignmentDirection.normalized * alignmentWeight;
        steering += cohesionDirection.normalized * cohesionWeight;

        //local leader
        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized;

        //target finding
        if (target != null)
        {
            follow -= transform.position;
            steering += follow.normalized * targetAggression;
        }
            
        //obstacle avoidance
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, localAreaRadius, LayerMask.GetMask("Shootable")))
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, speed)) * time;
    }
}