using System;
using System.Collections.Generic;
using Actors;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudController : MonoBehaviour
    {
        private BattleManager _battleManager;
        private GameObject _player;
        private Damageable _playerDamageable;

        private PlayerController _playerController;

        // private PersonController _playerPersonController;
        private StaminaController _playerStaminaController;

        public Slider healthBarSlider;
        public GameObject staminaBar;
        public GameObject staminaUnit;
        public TextMeshProUGUI currentTurnTMP;
        public GameObject currentTurnObject;
        public GameObject battleBeginsMessage;
        public GameObject endTurnButton;
        [SerializeField] private TextMeshProUGUI moneyTMP;

        private void Start()
        {
            if (_battleManager == null) _battleManager = GameObject.FindObjectOfType<BattleManager>();
            if (_battleManager != null)
            {
                _battleManager.onBattleStatusChange.AddListener(OnBattleBegin);
                _battleManager.onCurrentActorChange.AddListener(OnCurrentActorChange);
            }

            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null)
            {
                _player.TryGetComponent(out _playerStaminaController);
                _player.TryGetComponent(out _playerDamageable);
                _player.TryGetComponent(out _playerController);
            }

            if (_playerDamageable != null)
            {
                _playerDamageable.onCurrentHPChange.AddListener(OnPlayerHPChange);
            }

            if (_playerStaminaController != null)
            {
                _playerStaminaController.onCurrentStaminaChange.AddListener(OnPlayerStaminaChange);
            }


            // money manager part
            MoneyManager.Instance.onMoneyChanged.AddListener(currentMoney =>
            {
                moneyTMP.text = $"Money: {currentMoney}";
            });
            moneyTMP.text = $"Money: {MoneyManager.Instance.currentMoney}";
        }

        private void OnCurrentActorChange(IActorController currentActor)
        {
            if (currentTurnTMP != null)
            {
                var actor = (MonoBehaviour) currentActor;
                currentTurnTMP.text = $"Now acting: {actor.name}";
            }

            if (endTurnButton != null)
            {
                if (currentActor == _playerController)
                {
                    endTurnButton.SetActive(true);
                }
                else
                {
                    endTurnButton.SetActive(false);
                }
            }
        }

        private void OnPlayerHPChange(float current, float max)
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.value = current / max;
            }
        }

        private void OnPlayerStaminaChange(float current, float max)
        {
            foreach (Transform child in staminaBar.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = (int) current; i > 0; i--)
            {
                Instantiate(staminaUnit, staminaBar.transform);
            }
        }

        private void OnBattleBegin(bool isBattle)
        {
            if (battleBeginsMessage == null) return;
            if (isBattle)
            {
                staminaBar.transform.parent.gameObject.SetActive(true);
                battleBeginsMessage.SetActive(true);
                if (currentTurnObject != null) currentTurnObject.SetActive(true);
            }
            else
            {
                staminaBar.transform.parent.gameObject.SetActive(false);
                endTurnButton.SetActive(false);
                battleBeginsMessage.SetActive(false);
                if (currentTurnObject != null) currentTurnObject.SetActive(false);
            }
        }

        public void OnFinishMoveButton()
        {
            if (_playerController != null)
            {
                _playerController.EndTurn();
            }
        }

        private void OnPauseToggle(bool isPause)
        {
            if (isPause)
            {
                Debug.Log("Game paused");
            }
            else
            {
                Debug.Log("Game playing");
            }
        }
    }
}