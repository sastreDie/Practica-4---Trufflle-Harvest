using System;
using System.Collections.Generic;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [FormerlySerializedAs("Rigidbody")]
        [Header("References")]
        [SerializeField, Self]
        public Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineCamera freeLook;
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Self] private Health health;

        
        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f; 
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothtime = 0.2f;
        
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;
        
        [Header("Shooting")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float rateOfFire = 1f;
        [SerializeField] private float upAngle = 0.3f;
        private float nextFireTime;
        
        
        Transform mainCam;
        
        float currentSpeed;
        float velocity;
        float jumpVelocity;
        
        
        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        static readonly int Speed = Animator.StringToHash("Speed");
        
        StateMachine stateMachine;
        
        private bool isFiring;
        private bool isDead;
        
        private Vector3 startPosition;
        private Quaternion startRotation;



        void Awake()
        {
            mainCam = Camera.main.transform;
            freeLook.Follow = transform;
            freeLook.LookAt = transform;
            freeLook.OnTargetObjectWarped(transform, positionDelta:transform.position - freeLook.transform.position - Vector3.forward);
            
            rb.freezeRotation = true;
            
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers = new List<Timer> (capacity:2) {jumpTimer, jumpCooldownTimer};
            
            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
            
            //stateMachine = new StateMachine();
            stateMachine = new StateMachine();
            
            //declare states
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var attackState = new PlayerAttackState(this, animator);
            var deadState = new PlayerDeadState(this, animator);;

            //define transitions
            At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            At(jumpState, locomotionState, new FuncPredicate(() => !jumpTimer.IsRunning && groundChecker.isGrounded));
            At(locomotionState, attackState, new FuncPredicate(() => isFiring));
            At(attackState, locomotionState, new FuncPredicate(() => attackState.CanExitState()));;

            
            Any(deadState, new FuncPredicate(() => isDead));
            
            At(deadState, locomotionState, new FuncPredicate(() => !isDead));;

            stateMachine.SetState(locomotionState);
            
            startPosition = transform.position;
            startRotation = transform.rotation;

        }
        public void ResetPlayer()
        {
            isDead = false;
            input.EnablePlayerActions();
        
            // Reset position if needed
            transform.position = startPosition; // Define startPosition in Awake
            transform.rotation = startRotation; // Define startRotation in Awake
            
        
            // Re-enable components
            enabled = true;
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        
        void At(iState from, iState to, iPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(iState to, iPredicate condition) => stateMachine.AddAnyTransition(to,condition);

        void Start() => input.EnablePlayerActions();

        void OnEnable()
        {
            input.Jump += onJump;
            input.Fire += onFire;
            health.onDeath.AddListener(OnDeath);

        }

        void OnDisable()
        {
            input.Jump -= onJump;
            input.Fire -= onFire;
            health.onDeath.RemoveListener(OnDeath);

        }
        
        private void OnDeath()
        {
            if (isDead) return;
            isDead = true;
        
            // Disable input
            input.DisablePlayerActions();
        
            // State machine will automatically transition to dead state
        }


        void onJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.isGrounded)
            {
                jumpTimer.Start();
            } else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        void onFire(bool performed)
        {
            
            if (performed && Time.time > nextFireTime && groundChecker.isGrounded)
            {
                isFiring = true;
                FireProjectile();
                nextFireTime = Time.time + rateOfFire;
            }
            else
            {
                isFiring = false;
            }
        }

        private void FireProjectile()
        {
            Vector3 cameraDirection = mainCam.forward;
            cameraDirection.y += upAngle;
            cameraDirection.Normalize();
            
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(cameraDirection));
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetShooter(gameObject);
            }

        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();

            HandleTimers();
            UpdateAnimator();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        public void HandleJump() {
            
            // If not jumping and grounded, keep jump velocity at 0
            if (!jumpTimer.IsRunning && groundChecker.isGrounded) {
                jumpVelocity = 0f;
                return;
            }
            
            if (!jumpTimer.IsRunning) {
                // Gravity takes over
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
                jumpVelocity = Mathf.Max(jumpVelocity, -20f);

            }
            
            // Apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        public void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            var adjustedDirection =  Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            if (adjustedDirection.magnitude > 0f)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                currentSpeed = SmoothSpeed(adjustedDirection.magnitude);
            }else
            {
                currentSpeed = SmoothSpeed(0f);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            Vector3 velocity = adjustedDirection * moveSpeed * Time.deltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }

        private float SmoothSpeed(float value)
        {
            return Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothtime);
        }
        
    
        // Expose Rigidbody for the dead state
        public Rigidbody Rigidbody => rb;

    }
}