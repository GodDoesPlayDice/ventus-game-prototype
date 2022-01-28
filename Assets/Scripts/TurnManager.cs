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

    private void Start()
    {
        _actorsQueue = new Queue<ActorController>();
        PlayerActor = GameObject.Find("Player").GetComponent<ActorController>();
        AddLast(PlayerActor);
        CurrentActor = PlayerActor;
    }

    private void NextTurn()
    {
        if (_actorsQueue.Count <= 1)
        {
            CurrentActor = PlayerActor;
            return;
        }
        RemoveFirst();
        
        // check if current actor is dead
        var currentActor = _actorsQueue.Peek();
        if (currentActor.exitQueueOnNextTurn)
        {
            NextTurn();
        }
        else
        {
            // perform the action of the new first-in-line actor
            CurrentActor = currentActor;
            AddLast(currentActor);
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
        actor.onActionEnded.AddListener(NextTurn);
        return true;
    }

    private void RemoveFirst()
    {
        _actorsQueue.Dequeue();
        CurrentActor.onActionEnded.RemoveListener(NextTurn);
    }

    public void EnterCombatMode()
    {
    }

    public void ExitCombatMode()
    {
    }
}