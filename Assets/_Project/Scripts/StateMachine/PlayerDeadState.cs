using UnityEngine;

namespace Platformer
{
    public class PlayerDeadState : BaseState
    {
        

        public PlayerDeadState(PlayerController player, Animator anim) : base(player, anim)
        {
        }

        public override void onEnter()
        {
            anim.CrossFade(dieHash, crossFadeDuration);
            // Disable player movement
            if (player.rb != null)
            {
                player.rb.isKinematic = true;
            }
        }
        
    }
}