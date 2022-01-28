using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegenTime;
    [HideInInspector] public Animator animator;

    private float _currentStamina;
    private static readonly int AttackTrigger = Animator.StringToHash("attack");
    public void Attack(Damageable victim)
    {
        if (animator != null) animator.SetTrigger(AttackTrigger);
        victim.TakeDamage(damage);
    }
}