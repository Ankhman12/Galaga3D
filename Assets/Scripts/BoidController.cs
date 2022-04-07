using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int SwarmIndex { get; set; }
    [Header("Flocking Variables")]
    // Separation
    [SerializeField] private float noClumpingRadius = 100f;
    // Alignment and Cohesion
    [SerializeField] private float localAreaRadius = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float steeringSpeed = 100f;
    Vector3 steering = Vector3.zero;

    Vector3 separationDirection = Vector3.zero;
    int separationCount = 0;
    Vector3 alignmentDirection = Vector3.zero;
    int alignmentCount = 0;
    Vector3 cohesionDirection = Vector3.zero;
    int cohesionCount = 0;

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
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, localAreaRadius, LayerMask.GetMask("Default")))
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, speed)) * time;
    }
}