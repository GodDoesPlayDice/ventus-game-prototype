using System;
using System.Collections;
using UnityEngine;
using Enums;
using Interfaces;
using Unity.VisualScripting;
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

    private Vector3 _currentDestination;
    private Damageable _currentVictim;
    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _attacker);
        TryGetComponent(out _animationManager);
    }

    public void Act(Vector3 pos)
    {
        _currentDestination = pos;
        Act(ActorActions.Move);
    }

    public void Act(Damageable victim)
    {
        _currentVictim = victim;
        Act(ActorActions.Attack);
    }

    private void Act(ActorActions action)
    {
        if (actorState == ActorState.Acting) return;
        actorState = ActorState.Acting;
        switch (action)
        {
            case ActorActions.Move:
                // Debug.Log($"walking to {_currentDestination}");
                _walker.Walk(_currentDestination);
                StartCoroutine(CheckReachDestinationRoutine());
                break;
            case ActorActions.Attack:
                // Debug.Log($"attacked {_currentVictim.name}");
                _animationManager.AttackAnimation();
                _attacker.Attack(_currentVictim);
                StartCoroutine(CountAfterAttackRoutine());
                break;
            case ActorActions.Interact:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
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
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}