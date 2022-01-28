using System;
using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static GameModes CurrentGameMode;
    public static ActorController CurrentActor;
    public static ActorController PlayerActor;
    
    private Queue<ActorController> _actorsQueue;

    private void Awake()
    {
        _actorsQueue = new Queue<ActorController>();
    }

    private void Start()
    {
        PlayerActor = GameObject.Find("Player").GetComponent<ActorController>();
        AddLast(PlayerActor);
        CurrentActor = PlayerActor;
    }

    private void OnActionEnded(ActorController actor)
    {
        if (_actorsQueue.Count <= 1)
        {
            CurrentActor = PlayerActor;
            return;
        }
        RemoveFirst(actor);
        
        // check if current actor is dead
        var currentActor = _actorsQueue.Peek();
        if (currentActor.exitQueueOnNextTurn)
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
        Debug.Log($"actor added to queue {actor.name}");
        _actorsQueue.Enqueue(actor);
        actor.onActionEnded.AddListener(OnActionEnded);
        return true;
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