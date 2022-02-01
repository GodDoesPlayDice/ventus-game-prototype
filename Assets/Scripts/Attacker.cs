using System;
using UnityEngine;

[RequireComponent(typeof(HumanAnimationManager))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private float damage;
    [HideInInspector] public HumanAnimationManager animatorManager;
    [SerializeField] private float meleeDistance = 2.2f;

    private void Awake()
    {
        animatorManager = GetComponent<HumanAnimationManager>();
    }

    private static readonly int AttackTrigger = Animator.StringToHash("attack");
    public void Attack(Damageable victim)
    {
        Vector3 direction = (victim.transform.position - transform.position).normalized;
        if (Vector3.Distance(victim.transform.position, transform.position) > meleeDistance)
        {
            animatorManager.AttackAnimation(direction);
        }
        else
        {
            animatorManager.MeleeAttackAnimation(direction);
        }

        victim.TakeDamage(damage);
    }
}