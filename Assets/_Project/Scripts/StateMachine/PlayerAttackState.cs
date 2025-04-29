using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace Platformer
{
    public class PlayerAttackState : BaseState

    {
        private readonly PlayerController player;
        private readonly Animator animator;
        private const float crossFadeDuration = 0.1f;
        private float attackStartTime;
        private float attackAnimationLength;


        public PlayerAttackState(PlayerController player, Animator animator) : base(player, animator)
        {
            this.player = player;
            this.animator = animator;
            attackAnimationLength = 0.3f;

        }

        public override void onEnter()
        {
            Debug.Log("ass");
            base.onEnter();
            attackStartTime = Time.time;
            animator.CrossFade(attackHash, crossFadeDuration);

        }
        

        // Add this method to check if we can exit the attack state
        public bool CanExitState()
        {
            return Time.time >= attackStartTime + attackAnimationLength;
        }


    }
}