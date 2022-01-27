using System;
using UnityEngine;
using Enums;

public class ActorController : MonoBehaviour
{
    public Actor actorType = Actor.AI;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public Animator animator;

    private WalkerController _walkerController;
    private static readonly int Attack = Animator.StringToHash("attack");

    private void Awake()
    {
        TryGetComponent(out _walkerController);
    }

    public void Act(GameActions action)
    {
        switch (action)
        {
            case GameActions.Move:
                _walkerController.WalkTo(destination);
                break;
            case GameActions.Attack:
                if (animator != null) animator.SetTrigger(Attack);
                break;
            case GameActions.Interact:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }
}