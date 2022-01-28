using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
public class TurnManager : MonoBehaviour
{
    public Queue<ActorTurn> ActorTurns;

    private void Start()
    {
        
    }
}

public class ActorTurn
{
    public ActorActions Action;
    public Vector3 Position;
    public Damageable Damageable;
}