using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;

    public bool IsDead { get; private set; } = false;

    public UnityEvent<float, float> onCurrentHPChange;
    public UnityEvent onDeath;
    private void Start()
    {
        onCurrentHPChange ??= new UnityEvent<float, float>();
        currentHealth = maxHealth;
        onCurrentHPChange.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        var newHealth = Mathf.Clamp((currentHealth - damage), 0, maxHealth);
        if (newHealth <= 0)
        {
            IsDead = true;
            Die();
        }

        if (Math.Abs(currentHealth - newHealth) > 0.01f)
        {
            onCurrentHPChange.Invoke(newHealth, maxHealth);
        }
        currentHealth = newHealth;
    }

    private void Die()
    {
        Debug.Log($"death of {gameObject.name}", this);
        onDeath.Invoke();
    }
}