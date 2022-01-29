using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actors
{
    public class PlayerController : MonoBehaviour, IActorController
    {
        public Vector3 mousePosition;
        public Camera camera;
        public CursorController cursorController;
        
        private PersonController _personController;

        private bool _canAct = true;
        private bool _inBattle;
        private Action<bool> _endTurnCallback;

        private void Awake()
        {
            TryGetComponent(out _personController);
            //_cam = Camera.main;
            if (cursorController == null) cursorController = GameObject.FindObjectsOfType<CursorController>()[0];
        }

        public void OnFire(InputValue value)
        {
            Debug.Log("Handle mouse " + _canAct);
            if (_canAct)
            {
                HandleMouseClick();
            }
        }

        public void OnPause(InputValue value)
        {
            HandleTogglePause();
        }

        public void OnMousePosition(InputValue value)
        {
            var pos = value.Get<Vector2>();

            var objectPos = camera.ScreenToWorldPoint(pos);
            mousePosition = objectPos;
            cursorController.mousePosition = mousePosition;
        }


        private void HandleTogglePause()
        {
            if (GameManager.GameState != GameState.Pause)
            {
                GameManager.SetGameState(GameState.Pause);
            }
            else
            {
                GameManager.SetGameState(GameState.Play);
            }
        }
        
        private void HandleMouseClick()
        {
            
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            var objectTag = hit.collider?.gameObject?.tag;
            switch (objectTag)
            {
                case "Ground":
                    // _actorController.selectedDestination = mousePosition;
                    // _actorController.selectedAction = ActorActions.Move;
                    // _actorController.Act();
                    _personController.SetAction(ActorAction.Move(mousePosition, succeed =>
                    {
                        Debug.Log("Player " + succeed);
                        if (!succeed && _endTurnCallback != null)
                        {
                            _endTurnCallback(true);
                            _canAct = false;
                        }
                    }));
                    break;
                case "Enemy":
                    hit.collider.gameObject.TryGetComponent(out Damageable victim);
                    if (victim != null)
                    {
                        // Debug.Log("trying to attack");
                        // _actorController.selectedVictim = victim;
                        // _actorController.selectedAction = ActorActions.Attack;
                        // _actorController.Act();
                        _personController.SetAction(ActorAction.Attack(victim, succeed =>
                        {
                            Debug.Log("Player attack " + succeed);
                        }));
                    }
                    break;
                default:
                    break;
            }
        }

        public void Act(Action<bool> endTurn)
        {
            _endTurnCallback = endTurn;
            _canAct = true;
            _personController.ResetStamina();
        }

        public void SetInBattle(bool inBattle)
        {
            _inBattle = inBattle;
            if (!inBattle)
            {
                _canAct = true;
            }
            _personController.SetIgnoreStamina(!inBattle);
        }
    }
}