using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Enums;
using TMPro;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Walker))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(ActorController))]
public class EnemyController : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    [SerializeField] private float distToNoticePlayer;
    [SerializeField] private float distToForgetPlayer;
    [SerializeField] private float attackDistance;

    private Walker _walker;
    private ActorController _actorController;
    private Damageable _damageable;
    private TurnManager _turnManager;

    private bool _chasingPlayer = false;

    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _damageable);
        TryGetComponent(out _actorController);
        if (_turnManager == null) GameObject.FindObjectOfType<TurnManager>();
        // find player
        if (PlayerGameObject == null) PlayerGameObject = GameObject.Find("Player");
    }

    private void Start()
    {
        StartCoroutine(CheckPlayerPosition());
    }

    private void Battle(float distToPlayer)
    {
        _turnManager.AddLast(_actorController);
        if (distToPlayer > attackDistance)
        {
            _actorController.selectedDestination = PlayerGameObject.transform.position;
            _actorController.selectedAction = ActorActions.Move;
            _actorController.Act();
        }
        else
        {
            _actorController.selectedVictim = PlayerGameObject.GetComponent<Damageable>();
            _actorController.selectedAction = ActorActions.Attack;
            _actorController.Act();
        }
    }

    private void ExitBattle()
    {
        _actorController.exitQueueOnNextTurn = true;
    }

    private IEnumerator CheckPlayerPosition()
    {
        for (;;)
        {
            if (_damageable.IsDead)
            {
                yield break;
            }

            if (PlayerGameObject != null && _walker != null && _actorController != null)
            {
                var playerPos = PlayerGameObject.transform.position;

                var distance = Vector3.Distance(transform.position, playerPos);
                if (distance <= distToNoticePlayer)
                {
                    Battle(distance);
                }
                else if (distance >= distToForgetPlayer && _chasingPlayer)
                {
                    ExitBattle();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}