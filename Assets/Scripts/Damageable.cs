using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;

    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        var newHealth = Mathf.Clamp((currentHealth - damage), 0, maxHealth);
        if (newHealth <= 0)
        {
            IsDead = true;
            Die();
        }

        currentHealth = newHealth;
    }

    private void Die()
    {
        Debug.Log($"death of {gameObject.name}", this);
    }
}