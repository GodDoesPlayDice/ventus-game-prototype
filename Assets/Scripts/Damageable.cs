using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;

    
    private float _currentHealth = 100;

    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            onCurrentHPChange.Invoke(_currentHealth, maxHealth);
        }
    }

    public bool IsDead { get; private set; } = false;

    public UnityEvent<float, float> onCurrentHPChange;
    public UnityEvent onDeath;
    private void Start()
    {
        onCurrentHPChange ??= new UnityEvent<float, float>();
        //currentHealth = maxHealth;
        onCurrentHPChange.Invoke(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        var newHealth = Mathf.Clamp((_currentHealth - damage), 0, maxHealth);
        if (newHealth <= 0)
        {
            IsDead = true;
            Die();
        }

        if (Math.Abs(_currentHealth - newHealth) > 0.01f)
        {
            onCurrentHPChange.Invoke(newHealth, maxHealth);
        }
        _currentHealth = newHealth;
    }

    private void Die()
    {
        Debug.Log($"death of {gameObject.name}", this);
        onDeath.Invoke();
    }
}