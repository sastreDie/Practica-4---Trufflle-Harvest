using UnityEngine;

namespace Platformer
{
    public class EnemyBaseState : iState
    {
        protected readonly Enemy enemy;
        protected readonly Animator anim;
        
        protected static readonly int idlleHash = Animator.StringToHash("Idle");
        protected static readonly int runHash = Animator.StringToHash("Run");
        protected static readonly int walkHash = Animator.StringToHash("Walk");
        protected static readonly int attackHash = Animator.StringToHash("Attack");
        protected static readonly int getHitHash = Animator.StringToHash("GetHit");
        protected static readonly int dieHash = Animator.StringToHash("Die");
        
        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(Enemy enemy, Animator anim)
        {
            this.enemy = enemy;
            this.anim = anim;
        }
        public virtual void onEnter()
        {
            //no
        }

        public virtual void Update()
        {
            //no
        }

        public virtual void FixedUpdate()
        {
            //no
        }

        public virtual void onExit()
        {
            //no
        }
    }
}