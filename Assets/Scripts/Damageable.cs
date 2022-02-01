using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;


    private float _currentHealth;
    private bool healthInnerSet;

    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            healthInnerSet = true;
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
        if (!healthInnerSet) _currentHealth = maxHealth;
        onCurrentHPChange.Invoke(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

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
        
        TryGetComponent(out NavMeshAgent agent);
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}