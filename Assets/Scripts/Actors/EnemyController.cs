using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using Enums;
using UnityEngine;

namespace Actors
{
    public class EnemyController : MonoBehaviour, IActorController
    {
        
        public static GameObject PlayerGameObject { get; private set; }

        public float awareDistance = 10f;
        public float forgetDistance = 15f;
        
        
        //private Walker _walker;
        private PersonController _personController;
        //private Damageable _damageable;
        //private TurnManager _turnManager;

        private bool _inBattle;
        private Damageable _playerDamageable;

        private BattleManager _battleManager;
        

        private void Awake()
        {
            //TryGetComponent(out _walker);
            //TryGetComponent(out _damageable);
            TryGetComponent(out _personController);
            //if (_turnManager == null) _turnManager = GameObject.FindObjectOfType<TurnManager>();
            // find player
            if (PlayerGameObject == null)
            {
                PlayerGameObject = GameObject.Find("Player");
            }
            _playerDamageable = PlayerGameObject.GetComponent<Damageable>();

            _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_inBattle)
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
            _personController.ResetStamina();
            StartCoroutine(ContinueAct(endTurn));
        }

        private IEnumerator ContinueAct(Action<bool> endTurn)
        {
            yield return 0;
            if (IsPlayerForgot())
            {
                _battleManager.Unregister(this);
                endTurn(false);
                yield break;
            }
            _personController.SetAction(ActorAction.Attack(_playerDamageable, success =>
            {
                if (success)
                {
                    StartCoroutine(ContinueAct(endTurn));
                }
                else
                {
                    endTurn(true);
                }
            }));
        }

        public void SetInBattle(bool inBattle)
        {
            _inBattle = inBattle;
            _personController.SetIgnoreStamina(!inBattle);
        }
    }
}
