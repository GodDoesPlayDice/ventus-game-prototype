using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Actions;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Weapons;

namespace Actors
{
    public class PlayerController : MonoBehaviour, IActorController
    {
        public Vector3 mousePosition;
        public Camera camera;
        public CursorController cursorController;

        public List<GameObject> cameraList;

        private PersonController _personController;

        private bool _canAct = true;
        private bool _inBattle;
        private Action<bool> _endTurnCallback;
        private Damageable _damageable;
        private GameManager _gameManager;
        private AttackRadiusController _attackRadiusController;
        private WeaponHolder _weaponHolder;

        private Vector2 _prevMousePos;

        private void Awake()
        {
            TryGetComponent(out _personController);
            TryGetComponent(out _damageable);
            _attackRadiusController = GetComponent<AttackRadiusController>();
            _weaponHolder = GetComponent<WeaponHolder>();
            _damageable.onDeath.AddListener(Die);
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            if (cursorController == null) cursorController = GameObject.FindObjectsOfType<CursorController>()[0];
        }

        private void Update()
        {
            var objectPos = camera.ScreenToWorldPoint(_prevMousePos);
            mousePosition = objectPos;
            cursorController.mousePosition = mousePosition;
        }

        public void OnSwitchView(InputValue value)
        {
            if (cameraList.Count != 2) return;
            if (cameraList[0].activeSelf)
            {
                cameraList[0].SetActive(false);
                cameraList[1].SetActive(true);
            }
            else
            {
                cameraList[0].SetActive(true);
                cameraList[1].SetActive(false);
            }
        }

        public void OnFire(InputValue value)
        {
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
            _prevMousePos = value.Get<Vector2>();
        }


        private void HandleTogglePause()
        {
            if (_gameManager.GameState != GameState.Pause)
            {
                _gameManager.SetGameState(GameState.Pause);
            }
            else
            {
                _gameManager.SetGameState(GameState.Play);
            }
        }

        private void HandleMouseClick()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            // var hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            var hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            var hit = hits[0];
            foreach (var v in hits)
            {
                if (v.collider?.gameObject?.tag == "Interactable")
                {
                    hit = v;
                    break;
                }
                else if (v.collider?.gameObject?.tag == "Enemy")
                {
                    hit = v;
                    break;
                }
                else if (v.collider?.gameObject?.tag == "Ground")
                {
                    hit = v;
                }
            }

            var objectTag = hit.collider?.gameObject?.tag;
            Debug.Log("TAG: " + objectTag);
            switch (objectTag)
            {
                case "Ground":
                    SetShowAttackRadius(false);
                    // _actorController.selectedDestination = mousePosition;
                    // _actorController.selectedAction = ActorActions.Move;
                    // _actorController.Act();
                    _personController.SetAction(ActorAction.Move(mousePosition, succeed =>
                    {
                        if (succeed)
                        {
                            SetShowAttackRadius(true);
                        }
                        else
                        {
                            EndTurn();
                        }
                    }));
                    break;
                case "Enemy":
                    hit.collider.gameObject.TryGetComponent(out Damageable victim);
                    if (victim != null)
                    {
                        SetShowAttackRadius(false);
                        // Debug.Log("trying to attack");
                        // _actorController.selectedVictim = victim;
                        // _actorController.selectedAction = ActorActions.Attack;
                        // _actorController.Act();
                        _personController.SetAction(ActorAction.Attack(victim, succeed =>
                        {
                            SetShowAttackRadius(true);
                        }));
                    }

                    break;
                case "Interactable":
                    hit.collider.gameObject.TryGetComponent(out Interactable interactable);
                    if (interactable != null)
                    {
                        SetShowAttackRadius(false);
                        _personController.SetAction(ActorAction.Interact(interactable, suceed =>
                        {
                            SetShowAttackRadius(true);
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
            if (inBattle)
            {
                _personController.SetAction(null);
                SetShowAttackRadius(true);
            }
            else
            {
                _canAct = true;
            }

            _personController.SetIgnoreStamina(!inBattle);
        }

        public void EndTurn()
        {
            if (_endTurnCallback == null) return;
            _endTurnCallback(true);
            _personController.SetAction(null);
            _canAct = false;
            SetShowAttackRadius(false);
        }

        private void SetShowAttackRadius(bool active)
        {
            if (!_inBattle) return;
            
            if (active)
            {
                _attackRadiusController.attackRadius = _weaponHolder.ranged.distance * 2;
                _attackRadiusController.ShowAttackRadius();
            }
            else
            {
                _attackRadiusController.HideAttackRadius();
            }
        }

        private void Die()
        {
            _personController.Die();
            _gameManager.SetGameState(GameState.Dead);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            col.TryGetComponent(out SceneSwitchingPoint switcher);
            if (switcher != null)
            {
                switcher.SwitchScene();
            }
        }
    }
}