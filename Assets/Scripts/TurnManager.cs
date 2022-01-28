using System;
using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static GameModes CurrentGameMode;
    public static ActorController CurrentActor;
    
    private Queue<ActorController> _actorsQueue;

    private void Awake()
    {
        _actorsQueue = new Queue<ActorController>();
    }

    private void Start()
    {
        var player = GameObject.Find("Player").GetComponent<ActorController>();
        AddLast(player);
        CurrentActor = player;
    }

    private void OnActionEnded(ActorController actor)
    {
        if (_actorsQueue.Count <= 1)
        {
            CurrentActor = actor;
            return;
        }
        RemoveFirst(actor);
        
        // check if current actor is dead
        var currentActor = _actorsQueue.Peek();
        currentActor.TryGetComponent(out Damageable damageable);
        if (damageable == null || !damageable.IsDead)
        {
            OnActionEnded(actor);
        }
        else
        {
            // perform the action of the new first-in-line actor
            CurrentActor = currentActor;
            AddLast(actor);
        }
    }

    public bool AddLast(ActorController actor)
    {
        if (_actorsQueue.Contains(actor))
        {
            return false;
        }
        else
        {
            Debug.Log($"actor added to queue {actor.name}");
            _actorsQueue.Enqueue(actor);
            actor.onActionEnded.AddListener(OnActionEnded);
            return true;
        }
    }

    public void RemoveFirst(ActorController actor)
    {
        Debug.Log($"actor removed from queue {actor.name}");
        _actorsQueue.Dequeue();
        actor.onActionEnded.RemoveListener(OnActionEnded);
    }

    public void EnterCombatMode()
    {
    }

    public void ExitCombatMode()
    {
    }
}