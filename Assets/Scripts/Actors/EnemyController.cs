using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors
{
    public class EnemyController : MonoBehaviour, IActorController
    {
        
        public static GameObject PlayerGameObject { get; private set; }

        public float awareDistance = 10f;
        public float forgetDistance = 15f;

        public BoxCollider2D idleAreaCollider;
        
        
        //private Walker _walker;
        private PersonController _personController;
        //private Damageable _damageable;
        //private TurnManager _turnManager;

        private bool _inBattle;
        private bool _dead;
        private Damageable _damageable;
        private Damageable _playerDamageable;

        private BattleManager _battleManager;

        private Action<bool> _endTurn;

        private float _nextIdleMoveTime;
        private bool _idleCanMove = true;
        

        private void Awake()
        {
            TryGetComponent(out _personController);
            
            TryGetComponent(out _damageable);
            _damageable.onDeath.AddListener(Die);
            // find player
            if (PlayerGameObject == null)
            {
                PlayerGameObject = GameObject.FindGameObjectsWithTag("Player")[0];
            }
            _playerDamageable = PlayerGameObject.GetComponent<Damageable>();

            _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            CalcNextMoveTime();
        }

        // Update is called once per frame
        void Update()
        {
            if (_dead)
            {
                
            } else if (_inBattle)
            {
                
            }
            else
            {
                ActIdle();
            }
        }

        private void ActIdle()
        {
            if (IsPlayerInSight())
            {
                _inBattle = true;
                _battleManager.Register(this);
            }
            else if (_idleCanMove && _nextIdleMoveTime <= Time.time)
            {
                _idleCanMove = false;
                _personController.SetAction(ActorAction.Move(GetIdleWalkingPoint(), succeed =>
                {
                    _idleCanMove = true;
                    CalcNextMoveTime();
                }));
            }
        }

        private void CalcNextMoveTime()
        {
            _nextIdleMoveTime = Time.time + Random.Range(0.5f, 5f);
        }

        private Vector2 GetIdleWalkingPoint()
        {
            var bounds = idleAreaCollider.bounds;
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = Random.Range(bounds.min.y, bounds.max.y);
            return new Vector2(x, y);

        }

        private bool IsPlayerInSight()
        {
            return Vector3.Distance(transform.position, PlayerGameObject.transform.position) <= awareDistance;
        }

        private bool IsPlayerForgot()
        {
            return Vector3.Distance(transform.position, PlayerGameObject.transform.position) >= forgetDistance;
        }

        public void Act(Action<bool> endTurn)
        {
            _endTurn = endTurn;
            _personController.ResetStamina();
            StartCoroutine(nameof(ContinueAct));
        }

        private IEnumerator ContinueAct()
        {
            yield return 0;
            if (IsPlayerForgot())
            {
                LeaveBattle(true);
            }
            _personController.SetAction(ActorAction.Attack(_playerDamageable, success =>
            {
                if (success)
                {
                    StartCoroutine(nameof(ContinueAct));
                }
                else
                {
                    _endTurn(true);
                }
            }));
        }

        private void LeaveBattle(bool currentTurn)
        {
            _inBattle = false;
            _battleManager.Unregister(this);
            if (currentTurn)
            {
                _endTurn(false);
            }

            StopCoroutine(nameof(ContinueAct));
        }

        public void SetInBattle(bool inBattle)
        {
            if (inBattle)
            {
                _personController.SetAction(null);
            }
            _inBattle = inBattle;
            _personController.SetIgnoreStamina(!inBattle);
        }

        private void Die()
        {
            _personController.Die();
            LeaveBattle(false);
            _dead = true;
        }
    }
}
