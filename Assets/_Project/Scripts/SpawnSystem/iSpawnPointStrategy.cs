using UnityEngine;

namespace Platformer
{
    public interface iSpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}