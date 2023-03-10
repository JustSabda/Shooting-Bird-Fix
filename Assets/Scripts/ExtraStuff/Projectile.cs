using UnityEngine;
using Photon.Pun;

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


    [Header("Photon")]
    PhotonView view;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        if(view.IsMine)
        rb.velocity = transform.forward * speed;
    }
    private void Update()
    {
        if(view.IsMine)
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
            PhotonNetwork.Instantiate(explosionEffect.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }

        if (col.gameObject.tag == "Egg")
        {
            col.gameObject.GetComponent<Egg>().Damaged();
        }
        
    }


}

