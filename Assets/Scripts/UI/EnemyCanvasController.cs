using System;
using Actors;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyCanvasController : MonoBehaviour
    {
        public GameObject staminaAndHpPart;
        public Slider healthBarSlider;
        public Slider staminaBarSlider;

        public EnemyController actorController;
        public PersonController personController;
        public Damageable damageable;

        private BattleManager _battleManager;

        private void Awake()
        {
            GameObject.Find("BattleManager").TryGetComponent<BattleManager>(out _battleManager);
            if (_battleManager != null)
            {
                _battleManager.onBattleActorStatusChange.AddListener(OnBattleEnterExit);
            }
        }

        private void Start()
        {
            if (personController != null)
            {
                personController.onCurrentStaminaChange.AddListener(OnEnemyStaminaChange);
            }

            if (damageable != null)
            {
                damageable.onCurrentHPChange.AddListener(OnEnemyHpChange);
            }
        }

        private void OnBattleEnterExit(IActorController actor, bool entered)
        {
            if ((EnemyController) actor == actorController && entered)
            {
                staminaAndHpPart.SetActive(true);
            }
            else
            {
                staminaAndHpPart.SetActive(false);
            }
        }


        private void OnEnemyHpChange(float current, float max)
        {
            if (healthBarSlider != null)
                healthBarSlider.value = current / max;
        }

        private void OnEnemyStaminaChange(float current, float max)
        {
            if (staminaBarSlider != null)
                staminaBarSlider.value = current / max;
        }
    }
}