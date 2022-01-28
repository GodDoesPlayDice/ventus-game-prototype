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
    [SerializeField] private float attackDistance = 1;
    [SerializeField] private float singleWalkLength = 0.5f;

    private Walker _walker;
    private ActorController _actorController;
    private Damageable _damageable;
    private TurnManager _turnManager;

    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _damageable);
        TryGetComponent(out _actorController);
        if (_turnManager == null) _turnManager = GameObject.FindObjectOfType<TurnManager>();
        // find player
        if (PlayerGameObject == null) PlayerGameObject = GameObject.Find("Player");
    }

    private void Start()
    {
        StartCoroutine(CheckPlayerPosition());
    }

    private void Battle(float distToPlayer)
    {
        _turnManager.AddToQueue(_actorController);
        if (Mathf.Abs(distToPlayer - attackDistance) > attackDistance + singleWalkLength)
        {
            var dirToPlayer = (PlayerGameObject.transform.position - transform.position).normalized;
            Vector3 randomVector = new(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
            _actorController.selectedDestination = transform.position + (dirToPlayer * singleWalkLength) + randomVector;
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
        if (_turnManager.IsInQueueList(_actorController))
        {
            _turnManager.RemoveFromQueue(_actorController);
        }
    }

    private IEnumerator CheckPlayerPosition()
    {
        for (;;)
        {
            if (_damageable.IsDead)
            {
                ExitBattle();
                yield break;
            }

            if (PlayerGameObject != null && _walker != null && _actorController != null)
            {
                var playerPos = PlayerGameObject.transform.position;

                var distance = Vector3.Distance(transform.position, playerPos);
                if (distance < distToNoticePlayer ||
                    distance < distToForgetPlayer && _turnManager.IsInQueueList(_actorController))
                {
                    Battle(distance);
                }
                else
                {
                    ExitBattle();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}