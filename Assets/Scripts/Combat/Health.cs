using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathFXPrefab;

    [SerializeField] private GameObject _deathFXPrefab;
    [SerializeField] int _startingHealth = 3;
    [SerializeField] private GameObject _splatterPrefab;

    public static Action<Health> OnDeath; // allows actions to send values based off this specific script instance
    
    int _currentHealth;

    void Start() 
    {
        ResetHealth();
    }

    public void ResetHealth() 
    {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) 
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
