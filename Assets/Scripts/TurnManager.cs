using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static GameModes CurrentGameMode;
    public static ActorController CurrentActor;
    public static ActorController PlayerActor;

    private List<ActorController> _actorsQueueList;

    private void Start()
    {
        _actorsQueueList = new List<ActorController>();
        PlayerActor = GameObject.Find("Player").GetComponent<ActorController>();
        AddToQueue(PlayerActor);
        CurrentActor = PlayerActor;
    }

    private void NextTurn()
    {
        if (_actorsQueueList.Count <= 1)
        {
            CurrentActor = PlayerActor;
            return;
        }

        _actorsQueueList.Add(_actorsQueueList[0]);
        _actorsQueueList.RemoveAt(0);

        // check if current actor is dead
        CurrentActor = _actorsQueueList[0];
        Debug.Log($"current actor {CurrentActor}");
        // perform the action of the new first-in-line actor
    }

    public bool AddToQueue(ActorController actor)
    {
        if (_actorsQueueList.Contains(actor))
        {
            return false;
        }

        Debug.Log($"actor added to queue {actor.name}");
        _actorsQueueList.Add(actor);
        actor.onActionEnded.AddListener(NextTurn);
        return true;
    }

    public bool RemoveFromQueue(ActorController actor)
    {
        if (!_actorsQueueList.Contains(actor))
        {
            return false;
        }

        Debug.Log($"actor removed from queue {actor.name}");
        int index = _actorsQueueList.IndexOf(actor);
        _actorsQueueList.RemoveAt(index);
        actor.onActionEnded.RemoveListener(NextTurn);
        return true;
    }
}