using System;
using UnityEngine;

namespace Platformer
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 10f;
        [SerializeField] private float upwardsForce = 10f;
        [SerializeField] private float gravityMultiplier = 3f;
        
        [Header("Explosion Settings")]
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private LayerMask explosionLayers; 
        [SerializeField] private GameObject explosionEffectPrefab;


        private GameObject shooter;
        private Rigidbody rb;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        private void Start()
        {
            // Calculate the initial velocity combining forward and upward motion
            Vector3 initialVelocity = transform.forward * speed + Vector3.up * upwardsForce;
            rb.linearVelocity = initialVelocity;
        
            Destroy(gameObject, lifetime);
        }


        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            // Apply extra gravity
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == shooter) return;

            Explode();
        }

        private void Explode()
        {
            // Visual effect
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Find all enemies in radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider nearbyObject in colliders)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Die();
                }
            }

            Destroy(gameObject);
        }

        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

    }
}