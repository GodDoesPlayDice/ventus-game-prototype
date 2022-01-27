using System;
using System.Collections;
using UnityEngine;
using Enums;
using Unity.VisualScripting;

public class ActorController : MonoBehaviour
{
    public bool isActing = false;
    public ActorState actorState = ActorState.Idle;
    public Actor actorType = Actor.AI;

    private Vector3 _destination;
    public Vector3 Destination
    {
        get => _destination;
        set
        {
            if (actorState == ActorState.Acting) return;
            _destination = value;
        }
    }

    [HideInInspector] public Animator animator;
    private WalkerController _walkerController;
    
    private Coroutine _checkPositionCoroutine;


    private static readonly int Attack = Animator.StringToHash("attack");

    private void Awake()
    {
        TryGetComponent(out _walkerController);
    }

    public void Act(GameActions action)
    {
        if (actorState == ActorState.Acting) return;
        actorState = ActorState.Acting;
        switch (action)
        {
            case GameActions.Move:
                _walkerController.WalkTo(Destination);
                StartCoroutine(CheckPositionCoroutine());
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

    private IEnumerator CheckPositionCoroutine()
    {
        for (;;)
        {
            if (Vector3.Distance(transform.position, Destination) <= 0.05f)
            {
                Debug.Log("walk complete");
                actorState = ActorState.Idle;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}