using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;

        public EnemyChaseState(Enemy enemy, Animator anim, NavMeshAgent agent, Transform player) : base(enemy, anim)
        {
            this.agent = agent;
            this.player = player;
        }
        
        public override void onEnter()
        {
            Debug.Log("chase");
            anim.CrossFade(runHash, crossFadeDuration);
        }
        
        public override void Update()
        {
            agent.SetDestination(player.position);
        }
    }
}