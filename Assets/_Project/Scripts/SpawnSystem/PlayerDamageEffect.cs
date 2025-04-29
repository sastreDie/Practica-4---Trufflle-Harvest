using System.Collections;
using System.Collections.Generic;
using Platformer;
using UnityEngine;

public class PlayerDamageEffect : MonoBehaviour
{
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Color originalColor;
    private Health playerHealth;
    
    private void Start()
    {
        playerHealth = GetComponent<Health>();
        if (playerMaterial != null)
        {
            originalColor = playerMaterial.color;
        }
        
        playerHealth.onHealthChanged.AddListener(_ => FlashDamage());
    }
    
    private void FlashDamage()
    {
        if (playerMaterial != null)
        {
            StartCoroutine(DamageFlashRoutine());
        }
    }
    
    private IEnumerator DamageFlashRoutine()
    {
        playerMaterial.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        playerMaterial.color = originalColor;
    }
}