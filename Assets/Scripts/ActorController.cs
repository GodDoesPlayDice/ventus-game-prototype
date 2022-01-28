using System;
using System.Collections;
using UnityEngine;
using Enums;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class ActorController : MonoBehaviour
{
    public ActorState actorState = ActorState.Idle;
    public Actor actorType = Actor.AI;

    private Walker _walker;
    private Attacker _attacker;

    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _attacker);
    }

    public void Act(GameActions action, Vector3 v)
    {
        if (actorState == ActorState.Acting) return;
        actorState = ActorState.Acting;
        switch (action)
        {
            case GameActions.Move:
                _walker.Walk(v);
                StartCoroutine(CheckReachDestinationCoroutine());
                break;
            case GameActions.Attack:
                break;
            case GameActions.Interact:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    private IEnumerator CheckReachDestinationCoroutine()
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
}