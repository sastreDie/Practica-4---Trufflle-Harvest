using UnityEngine;

namespace Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator anim) : base(player, anim)
        {
        }

        public override void onEnter()
        {
            Debug.Log("JumpState.onEnter");
            anim.CrossFade(jumpHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.HandleJump();
            player.HandleMovement();
        }
    }
}