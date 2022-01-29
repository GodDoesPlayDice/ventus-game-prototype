using System;
using UnityEngine;

[RequireComponent(typeof(HumanAnimationManager))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private float damage;
    [HideInInspector] public HumanAnimationManager animatorManager;

    private void Awake()
    {
        animatorManager = GetComponent<HumanAnimationManager>();
    }

    private static readonly int AttackTrigger = Animator.StringToHash("attack");
    public void Attack(Damageable victim)
    {
        Vector3 direction = (victim.transform.position - transform.position).normalized;
        animatorManager.AttackAnimation(direction);
        victim.TakeDamage(damage);
    }
}