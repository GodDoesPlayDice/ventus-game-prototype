using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Enums;
using TMPro;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private TurnManager _turnManager;
    private Damageable _playerDamageable;

    public Slider healthBarSlider;
    public Slider staminaBarSlider;
    public TextMeshProUGUI currentTurnTMP;
    public GameObject battleBeginsMessage;

    private void Start()
    {
        if (_turnManager == null) _turnManager = GameObject.FindObjectOfType<TurnManager>();
        if (_turnManager != null)
        {
            _turnManager.onQueueChange.AddListener(OnQueueChange);
        }

        if (_playerDamageable == null) _playerDamageable = GameObject.Find("Player").GetComponent<Damageable>();
        if (_playerDamageable != null)
        {
            _playerDamageable.onCurrentHPChange.AddListener(OnPlayerHPChange);
        }
    }

    private void OnQueueChange(List<ActorController> queueList)
    {
        if (currentTurnTMP != null)
        {
            if (TurnManager.CurrentActor != null)
                currentTurnTMP.text = $"Now acting: {TurnManager.CurrentActor.gameObject.name}";
        }
    }

    private void OnPlayerHPChange(float newHp, float maxHp)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = newHp / maxHp;
        }
    }
    
    private void OnPlayerStaminaChange(float newStamina, float maxStamina)
    {
        if (staminaBarSlider != null)
        {
            staminaBarSlider.value = newStamina / newStamina;
        }
    }

    private void OnBattleBegin()
    {
        if (battleBeginsMessage != null)
        {
            battleBeginsMessage.SetActive(true);
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