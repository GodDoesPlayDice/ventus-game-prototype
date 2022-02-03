using System;
using UnityEngine;
using Weapons;

[RequireComponent(typeof(HumanAnimationManager))]
public class Attacker : MonoBehaviour
{
    //[SerializeField] private float damage;
    [HideInInspector] public HumanAnimationManager animatorManager;
    //[SerializeField] private float meleeDistance = 2.2f;

    private void Awake()
    {
        animatorManager = GetComponent<HumanAnimationManager>();
    }

    private static readonly int AttackTrigger = Animator.StringToHash("attack");
    public void Attack(Damageable victim, WeaponData weapon)
    {
        Vector3 direction = (victim.transform.position - transform.position).normalized;
        animatorManager.AttackAnimation(direction, weapon.prefix);

        victim.TakeDamage(weapon.damage);
    }
}