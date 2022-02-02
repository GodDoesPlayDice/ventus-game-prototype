using System;
using Actors;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace UI
{
    public class EnemyCanvasController : MonoBehaviour
    {
        public GameObject staminaAndHpPart;
        public Slider healthBarSlider;
        public GameObject staminaBar;
        public GameObject staminaUnit;

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
            if ((EnemyController) actor == actorController)
            {
                staminaAndHpPart.SetActive(entered);
                
                // TODO: replace with something better
                if (personController != null)
                {
                    OnEnemyStaminaChange(personController._stamina, personController.maxStamina);
                }
            }
        }


        private void OnEnemyHpChange(float current, float max)
        {
            if (healthBarSlider != null)
                healthBarSlider.value = current / max;
        }

        private void OnEnemyStaminaChange(float current, float max)
        {
            foreach (Transform child in staminaBar.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = (int)current; i > 0; i--)
            {
                Instantiate(staminaUnit, staminaBar.transform);
            }
        }
    }
}