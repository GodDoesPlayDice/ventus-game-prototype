using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static GameModes CurrentGameMode;
    public static ActorController CurrentActor;
    public static ActorController PlayerActor;

    private List<ActorController> _actorsQueueList;

    public UnityEvent<List<ActorController>> onQueueChange;

    private void Awake()
    {
        _actorsQueueList = new List<ActorController>();
        onQueueChange ??= new UnityEvent<List<ActorController>>();
    }

    private void Start()
    {
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
        // Debug.Log($"current actor {CurrentActor}");
        // perform the action of the new first-in-line actor
    }

    public bool AddToQueue(ActorController actor)
    {
        bool result = true;
        if (_actorsQueueList.Contains(actor))
        {
            result = false;
        }
        else
        {
            // Debug.Log($"actor added to queue {actor.name}");
            _actorsQueueList.Add(actor);
            actor.onActionEnded.AddListener(NextTurn);
            onQueueChange.Invoke(_actorsQueueList);
        }
        return result;
    }

    public bool RemoveFromQueue(ActorController actor)
    {
        bool result = true;
        if (_actorsQueueList.Contains(actor))
        {
            result = false;
        }
        else
        {
            // Debug.Log($"actor removed from queue {actor.name}");
            int index = _actorsQueueList.IndexOf(actor);
            _actorsQueueList.RemoveAt(index);
            actor.onActionEnded.RemoveListener(NextTurn);
            onQueueChange.Invoke(_actorsQueueList);
        }
        return result;
    }

    public bool IsInQueueList(ActorController actor)
    {
        return _actorsQueueList != null && _actorsQueueList.Contains(actor);
    }
}