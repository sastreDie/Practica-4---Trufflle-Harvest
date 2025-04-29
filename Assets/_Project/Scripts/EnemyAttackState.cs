using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;

        public EnemyAttackState(Enemy enemy, Animator anim, NavMeshAgent agent, Transform player) : base(enemy, anim)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void onEnter()
        {
            Debug.Log("Attack");
            anim.CrossFade(attackHash, crossFadeDuration);
        }
        
        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }
    }
}