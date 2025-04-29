using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Health playerHealth;

        private void Start()
        {
            // Subscribe to health changes
            if (playerHealth != null)
            {
                playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
            }
        }

        private void OnDestroy()
        {
            if (playerHealth != null)
            {
                playerHealth.onHealthChanged.RemoveListener(UpdateHealthBar);
            }
        }

        private void UpdateHealthBar(float healthPercentage)
        {
            fillImage.fillAmount = healthPercentage;
        }
    }
}