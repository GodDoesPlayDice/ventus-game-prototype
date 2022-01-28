using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Walker))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(ActorController))]
public class EnemyController : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    [SerializeField] private static float _distToNoticePlayer;
    [SerializeField] private static float _distToForgetPlayer;

    private Walker _walker;
    private ActorController _actorController;
    private Damageable _damageable;

    private bool _chasingPlayer = false;
    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _damageable);
        TryGetComponent(out _actorController);
        // find player
        if (PlayerGameObject == null) PlayerGameObject = GameObject.Find("Player");
    }

    private void Start()
    {
        StartCoroutine(CheckPlayerPosition());
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
                if (distance <= _distToNoticePlayer)
                {
                    _chasingPlayer = true;
                }
                else if (distance >= _distToForgetPlayer && _chasingPlayer)
                {
                    _chasingPlayer = false;
                    Debug.Log("Player lost for enemy", this);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}