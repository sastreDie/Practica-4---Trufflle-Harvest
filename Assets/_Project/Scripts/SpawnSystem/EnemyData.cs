using UnityEngine;

namespace Platformer
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Platformer/Enemy Data")]
    public class EnemyData : EntityData
    {
        public int score;
    }
}