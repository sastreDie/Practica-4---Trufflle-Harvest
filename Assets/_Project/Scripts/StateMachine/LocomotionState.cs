using UnityEngine;

namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator anim) : base(player, anim)
        {
        }
        
        public override void onEnter()
        {
            anim.CrossFade(locomotionHash, crossFadeDuration);
        }
        
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}