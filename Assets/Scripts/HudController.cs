using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HudController : MonoBehaviour
{
    private void Start()
    {
        GameManager.OnGameStateChange.AddListener((GameState arg) =>
        {
            var isPause = arg == GameState.Pause ? true : false;
            OnPauseToggle(isPause);
        });
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