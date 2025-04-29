using System;
using UnityEngine;

namespace Platformer
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float lifetime = 2f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}