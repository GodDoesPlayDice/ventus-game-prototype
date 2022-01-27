using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    private float _currentHealth;

    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        var newHealth = Mathf.Clamp((_currentHealth - damage), 0, maxHealth);
        if (newHealth <= 0)
        {
            IsDead = true;
            Die();
        }

        _currentHealth = newHealth;
    }

    private void Die()
    {
        Debug.Log($"death of {gameObject.name}", this);
    }
}