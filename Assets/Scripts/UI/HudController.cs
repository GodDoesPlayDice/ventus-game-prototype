using System.Collections.Generic;
using Actors;
using TMPro;
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
        private PersonController _playerPersonController;

        public Slider healthBarSlider;
        public Slider staminaBarSlider;
        public TextMeshProUGUI currentTurnTMP;
        public GameObject currentTurnObject;
        public GameObject battleBeginsMessage;
        public GameObject endTurnButton;

        private void Start()
        {
            if (_battleManager == null) _battleManager = GameObject.FindObjectOfType<BattleManager>();
            if (_battleManager != null)
            {
                _battleManager.onBattleStatusChange.AddListener(OnBattleBegin);
                _battleManager.onCurrentActorChange.AddListener(OnCurrentActorChange);
            }

            _player = GameObject.Find("Player");
            if (_player != null)
            {
                _player.TryGetComponent(out _playerPersonController);
                _player.TryGetComponent(out _playerDamageable);
                _player.TryGetComponent(out _playerController);
            }

            if (_playerDamageable != null)
            {
                _playerDamageable.onCurrentHPChange.AddListener(OnPlayerHPChange);
            }

            if (_playerPersonController != null)
            {
                _playerPersonController.onCurrentStaminaChange.AddListener(OnPlayerStaminaChange);
            }
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
                if ((PlayerController) currentActor == _playerController)
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
            if (staminaBarSlider != null)
            {
                staminaBarSlider.value = current / max;
            }
        }

        private void OnBattleBegin(bool isBattle)
        {
            if (battleBeginsMessage == null) return;
            if (isBattle)
            {
                battleBeginsMessage.SetActive(true);
                if (currentTurnObject != null) currentTurnObject.SetActive(true);
            }
            else
            {
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