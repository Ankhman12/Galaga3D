using UnityEngine;
using UnityEngine.VFX;

public class beetleProjectExplosion : MonoBehaviour
{
    [SerializeField] private float explosionTime = 5f;
    [SerializeField] private float damage = 3f;
    private float timer = 0;
    [SerializeField] private ParticleSystem normalParticles;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private VisualEffect explosion;
    [SerializeField] private float radius = 50f;
    private bool exploded = false;

    private void FixedUpdate()
    {
        if (timer >= explosionTime && !exploded)
        {
            Explosion();
        }
        if (timer >= explosionTime + 1)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.gameObject.tag == "Player")
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        normalParticles.Stop();
        Destroy(trail.gameObject);
        explosion.Play();
        Collider[] collisions = Physics.OverlapSphere(this.transform.position, radius);
        foreach(Collider c in collisions)
        {
            if (c.gameObject.tag == "Player")
            {
                var shipMov = FindObjectOfType<ShipMovement>();
                shipMov.Damage(damage);
                GameManager.Instance.hurtPlayer();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
