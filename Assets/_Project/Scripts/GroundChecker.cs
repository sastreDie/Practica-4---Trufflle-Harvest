using UnityEngine;

namespace Platformer
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] float groundDistance = 0.1f;
        [SerializeField] LayerMask groundLayers;
        
        
        public bool isGrounded { get; private set; }

        void Update()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayers);
        }
    }
}