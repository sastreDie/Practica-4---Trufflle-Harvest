using UnityEngine;

namespace Platformer
{
    public abstract class BaseState : iState
    {
        protected readonly PlayerController player;
        protected readonly Animator anim;
        
        protected static readonly int locomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int jumpHash = Animator.StringToHash("Jump");
        protected static readonly int attackHash = Animator.StringToHash("Attack");
        protected static readonly int dieHash = Animator.StringToHash("Die");
        
        protected const float crossFadeDuration = 0.1f;

        protected BaseState(PlayerController player, Animator anim)
        {
            this.player = player;
            this.anim = anim;
        }
        
        public virtual void onEnter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

        public virtual void onExit()
        {
            Debug.Log("BaseState.onExit");
        }
    }
}