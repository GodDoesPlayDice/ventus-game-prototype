using System;
using System.Collections;
using UnityEngine;
using Enums;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class ActorController : MonoBehaviour
{
    public ActorState actorState = ActorState.Idle;
    [SerializeField] private ActorTypes actorType = ActorTypes.AI;
    [SerializeField] private float attackDuration = 1f;
    private float _lastAttackTime;

    private Walker _walker;
    private Attacker _attacker;
    private IActorAnimationManager _animationManager;

    public ActorActions selectedAction;
    public Vector3 selectedDestination;
    public Damageable selectedVictim;

    public UnityEvent onActionEnded;
    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _attacker);
        TryGetComponent(out _animationManager);
    }

    private void Start()
    {
        onActionEnded ??= new UnityEvent();
    }
    

    public void Act()
    {
        if (TurnManager.CurrentActor != this) return;

        if (actorState == ActorState.Acting) return;
        actorState = ActorState.Acting;
        switch (selectedAction)
        {
            case ActorActions.Move:
                // Debug.Log($"walking to {_currentDestination}");
                _walker.Walk(selectedDestination);
                StartCoroutine(CheckReachDestinationRoutine());
                break;
            case ActorActions.Attack:
                // Debug.Log($"attacked {_currentVictim.name}");
                _animationManager.AttackAnimation();
                _attacker.Attack(selectedVictim);
                StartCoroutine(CountAfterAttackRoutine());
                break;
            case ActorActions.Interact:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(selectedAction), selectedAction, null);
        }
    }

    private IEnumerator CheckReachDestinationRoutine()
    {
        while (true)
        {
            if (!_walker.IsReachedDestination())
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                actorState = ActorState.Idle;
                onActionEnded.Invoke();
                yield break;
            }
        }
    }

    private IEnumerator CountAfterAttackRoutine()
    {
        _lastAttackTime = Time.time;
        for (;;)
        {
            if (Time.time - _lastAttackTime >= attackDuration)
            {
                actorState = ActorState.Idle;
                onActionEnded.Invoke();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}