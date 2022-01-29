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
        animatorManager.AttackAnimation();
        victim.TakeDamage(damage);
    }
}