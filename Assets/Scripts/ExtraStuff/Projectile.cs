using UnityEngine;


public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int hitcount = 3;

    [SerializeField] private float _triggerForce = 0.5f;
    [SerializeField] private float _explosionRadius =5;
    [SerializeField] private float _explosionForce = 500;

    private Vector3 lastvelocity;
    private Rigidbody rb;

    private bool destoryed = true;
    private int hit;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * speed;
    }
    private void Update()
    {
        lastvelocity = rb.velocity;
    }
    private void OnCollisionEnter(Collision col)
    {

        var surroundingObject = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var obj in surroundingObject)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }

        var speed = lastvelocity.magnitude;
        var direction = Vector3.Reflect(lastvelocity.normalized, col.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0f);

        hit += 1;



        if (hit == hitcount)
        {
            destoryed = false;
        }

        if (destoryed == false)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

}

