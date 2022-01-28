using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;

public class HumanAnimationManager : MonoBehaviour, IActorAnimationManager
{
    private static readonly int WalkBool = Animator.StringToHash("walk");
    private static readonly int UpBool = Animator.StringToHash("up");
    private static readonly int AttackTrigger = Animator.StringToHash("attack");

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

    public void AttackAnimation()
    {
        animator.SetTrigger(AttackTrigger);
    }

    public void MovementAnimation()
    {
        if (animator == null || _navMeshAgent == null || spriteRenderer == null) return;

        if (_navMeshAgent.velocity.magnitude >= 0.01f)
        {
            animator.SetBool(WalkBool, true);
            if (_navMeshAgent.velocity.y >= 0.2f)
            {
                animator.SetBool(UpBool, true);
            }
            else if (_navMeshAgent.velocity.y <= -0.2f)
            {
                animator.SetBool(UpBool, false);
            }

            if (_navMeshAgent.velocity.x >= 0.2f)
            {
                spriteRenderer.flipX = false;
            }
            else if (_navMeshAgent.velocity.x <= -0.2f)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool(WalkBool, false);
        }
    }
}