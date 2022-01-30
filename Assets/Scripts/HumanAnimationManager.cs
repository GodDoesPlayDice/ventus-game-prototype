using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;

public class HumanAnimationManager : MonoBehaviour, IActorAnimationManager
{
    private static readonly int WalkBool = Animator.StringToHash("walk");
    private static readonly int UpBool = Animator.StringToHash("up");
    private static readonly int AttackTrigger = Animator.StringToHash("attack");
    private static readonly int DeathTrigger = Animator.StringToHash("death");

    private NavMeshAgent _navMeshAgent;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out _navMeshAgent);
    }

    private void Update()
    {
        MovementAnimation();
    }

    public void AttackAnimation(Vector3 direction)
    {
        UpAndFlip(direction);
        animator.SetTrigger(AttackTrigger);
    }

    private static bool IsUp(Vector3 v)
    {
        return v.y >= 0f;
    }

    private static bool IsFlip(Vector3 v)
    {
        return v.x <= 0f;
    }

    private void UpAndFlip(Vector3 v)
    {
        animator.SetBool(UpBool, IsUp(v));
        spriteRenderer.flipX = IsFlip(v);
    }

    public void AttackAnimation()
    {
        throw new System.NotImplementedException();
    }

    public void MovementAnimation()
    {
        if (animator == null || _navMeshAgent == null || spriteRenderer == null) return;

        if (_navMeshAgent.velocity.magnitude >= 0.01f)
        {
            animator.SetBool(WalkBool, true);
            UpAndFlip(_navMeshAgent.velocity);
        }
        else
        {
            animator.SetBool(WalkBool, false);
        }
    }

    public void DeathAnimation()
    {
        animator.SetTrigger(DeathTrigger);
    }
}