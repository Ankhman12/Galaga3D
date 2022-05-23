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
    [SerializeField] private float radius = 54f;
    public LayerMask l;
    private bool exploded = false;

    private void FixedUpdate()
    {
        if (timer >= explosionTime && !exploded)
        {
            Explosion();
        }
        if (timer >= explosionTime + .5)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!exploded && collision.gameObject.CompareTag("Player"))
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
        Collider[] collisions = Physics.OverlapSphere(this.transform.position, radius, l);
        if(collisions.Length > 0)
        {
            var shipMov = FindObjectOfType<ShipMovement>();
            shipMov.Damage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
