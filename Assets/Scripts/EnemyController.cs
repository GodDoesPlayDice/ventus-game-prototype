using System;
using UnityEngine;

[RequireComponent(typeof(WalkerController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float distToNoticePlayer;
    [SerializeField] private float distToForgetPlayer;

    private WalkerController _walkerController;
    private GameObject _player;
    private PlayerController _playerController;
    private Vector3 _playerPosition;

    private void Awake()
    {
        TryGetComponent(out _walkerController);
        // find player
        _player = GameObject.Find("Player");
        if (_player != null) _player.TryGetComponent(out _playerController);
        // subscribe to player position
        if (_playerController != null) _playerController.onChangePosition.AddListener(OnPlayerPositionChange);
    }

    private void OnPlayerPositionChange(Vector3 playerPos)
    {
        if (_walkerController == null) return;
        float distance = Vector3.Distance(transform.position, playerPos);
        if (distance <= distToNoticePlayer)
        {
            _walkerController.WalkTo(playerPos);
        } else if (distance >= distToForgetPlayer)
        {
            Debug.Log("Player lost for enemy", this);
        }
    }
}