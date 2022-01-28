using UnityEngine;
using UnityEngine.AI;

public class HumanAnimationManager : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Up = Animator.StringToHash("up");

    private NavMeshAgent _navMeshAgent;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out _navMeshAgent);
    }

    private void Update()
    {
        if (animator == null || _navMeshAgent == null || spriteRenderer == null) return;

        if (_navMeshAgent.velocity.magnitude >= 0.01f)
        {
            animator.SetBool(Walk, true);
            if (_navMeshAgent.velocity.y >= 0.2f)
            {
                animator.SetBool(Up, true);
            }
            else if (_navMeshAgent.velocity.y <= -0.2f)
            {
                animator.SetBool(Up, false);
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
            animator.SetBool(Walk, false);
        }
    }
}