using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    
    
    
    private int _currentHealth;


    
    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;
    
    
    
    private void Awake()
    {
        ResetHealth();
    }


    
    public void TakeDamage(int amount)
    {
        if (_currentHealth <= 0)
        {
            return;
        }
        
        _currentHealth -= amount;
        
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth > 0)
        {
            return;
        }
        
        _currentHealth = 0;
        Die();
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}