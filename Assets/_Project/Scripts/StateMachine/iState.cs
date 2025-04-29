using UnityEngine;

namespace Platformer
{
    public interface iState
    {
        void onEnter();
        void Update();
        void FixedUpdate();
        void onExit();
    }
}
