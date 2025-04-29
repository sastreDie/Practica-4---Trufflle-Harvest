using Platformer;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(Enemy enemy, Animator anim) : base(enemy, anim)
    {
    }

    public override void onEnter()
    {
        Debug.Log("Died");
        anim.CrossFade(dieHash, crossFadeDuration);
    }
}