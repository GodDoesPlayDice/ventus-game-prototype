using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Enums;
using TMPro;

public class HudController : MonoBehaviour
{
    private TurnManager _turnManager;
    private Damageable _playerDamageable;
    public TextMeshProUGUI hpTMP;
    public TextMeshProUGUI currentTurnTMP;

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
            currentTurnTMP.text = $"Current turn: {queueList[0]}";
        }
    }

    private void OnPlayerHPChange(float newHp)
    {
        if (hpTMP != null)
        {
            hpTMP.text = $"HP: {newHp.ToString(CultureInfo.InvariantCulture)}";
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