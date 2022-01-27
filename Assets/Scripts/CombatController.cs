using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegenTime;


    private float _currentStamina;
    
    public void Attack(Damageable victim)
    {
        victim.TakeDamage(damage);
    }
}