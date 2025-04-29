using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f;
        [SerializeField] float detectionRadius = 10f;
        [SerializeField] float innerDetectionRadius = 5f;
        [SerializeField] float detectionCooldown = 1f;
        [SerializeField] float attackRange = 2f;
        
        public Transform player { get; private set; }
        CountdownTimer detectionTimer;
        public Health playerHealth { get; private set; } 

        
        IDetectionStrategy detectionStrategy;

        private void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            FindPlayer();
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void FindPlayer()
        {
            if (player == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                    playerHealth = playerObj.GetComponent<Health>(); // Cache the Health component
                
                    if (playerHealth == null)
                    {
                        Debug.LogWarning("PlayerDetector: Player doesn't have a Health component!");
                    }
                }
                else
                {
                    Debug.LogWarning("PlayerDetector: Cannot find player with 'Player' tag!");
                }
            }
        }


        // Add this method
        private void OnEnable()
        {
            FindPlayer();
        }

        public bool canDetectPlayer()
        {
            // Add a null check
            if (player == null)
            {
                FindPlayer();
                return false;
            }
            return detectionTimer.IsRunning || detectionStrategy.Execute(player, transform, detectionTimer);
        }


        public bool canAttackPlayer()
        {
            var directionToPlayer = player.position - transform.position;
            return directionToPlayer.magnitude < attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;
        
        void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            // Calculate our cone directions
            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;

            // Draw lines to represent the cone
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }
    }
}