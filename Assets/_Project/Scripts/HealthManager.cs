using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    private float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Health initialized at {currentHealth}"); // Add debug log
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(0f, currentHealth - damageAmount);
        Debug.Log($"Taking damage: {damageAmount}. Health now: {currentHealth}"); // Add debug log
        
        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!"); // Add debug log
        onDeath?.Invoke();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

}