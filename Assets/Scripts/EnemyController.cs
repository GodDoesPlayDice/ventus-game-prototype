using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Walker))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(ActorController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float distToNoticePlayer;
    [SerializeField] private float distToForgetPlayer;

    private Walker _walker;
    private Damageable _damageable;
    private GameObject _player;
    private Vector3 _playerPosition;

    private bool _chasingPlayer = false;
    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _damageable);
        // find player
        _player = GameObject.Find("Player");
    }

    private void OnPlayerPositionChange(Vector3 playerPos)
    {
        if (_walker == null) return;
        float distance = Vector3.Distance(transform.position, playerPos);
        if (distance <= distToNoticePlayer)
        {
            _chasingPlayer = true;
            _walker.Walk(playerPos);
        }
        else if (distance >= distToForgetPlayer && _chasingPlayer)
        {
            _chasingPlayer = false;
            Debug.Log("Player lost for enemy", this);
        }
    }
}