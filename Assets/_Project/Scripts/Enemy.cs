using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Platformer
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector PlayerDetector;
        [SerializeField, Child] Animator anim;
        
        [SerializeField] float wanderRadius = 10f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damageAttack = 10f;
        
        StateMachine stateMachine;

        CountdownTimer attackTimer;
        
        public static event System.Action OnEnemyDied; // Add this event
    
        private bool isDead = false;


        void OnValidate() => this.ValidateRefs();

        private void Start()
        {
            gameObject.tag = "Enemy";

            attackTimer = new CountdownTimer(timeBetweenAttacks);
            stateMachine = new StateMachine();
            
            var wanderState = new EnemyWanderState(this, anim, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, anim, agent, PlayerDetector.player);
            var attackState = new EnemyAttackState(this, anim, agent, PlayerDetector.player); 
            var dieState = new EnemyDeathState(this, anim);

            At(wanderState, chaseState, new FuncPredicate(() => PlayerDetector.canDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !PlayerDetector.canDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => PlayerDetector.canAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !PlayerDetector.canAttackPlayer()));
            At(attackState, dieState, new FuncPredicate(() => isDead));
            At(chaseState, dieState, new FuncPredicate(() => isDead));
            At(wanderState, dieState, new FuncPredicate(() => isDead));
            
            stateMachine.SetState(wanderState);
        }
        
        void At(iState from, iState to, iPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(iState to, iPredicate conditon) => stateMachine.AddAnyTransition(to, conditon);

        void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void Attack()
        {
            if (attackTimer.IsRunning) return;

            if (PlayerDetector.playerHealth != null)
            {
                PlayerDetector.playerHealth.TakeDamage(damageAttack);  // Remove Time.deltaTime since we're using the timer
                Debug.Log($"Attacking player. Damage dealt: {damageAttack}"); // Add debug log
            }

            attackTimer.Start();
        }

        
        public void Die()
        {
            if (isDead) return;
            isDead = true;

            // Stop all current behavior
            agent.enabled = false;
    
            // Disable all colliders
            foreach (var collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }
    
            stateMachine.SetState(new EnemyDeathState(this, anim));

            // Notify spawn system
            OnEnemyDied?.Invoke();

            // Destroy after animation
            Destroy(gameObject, 2f);
        }

    }
}