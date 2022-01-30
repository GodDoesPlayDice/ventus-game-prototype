using System;
using System.Collections;
using UnityEngine;
using Enums;
using UI;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public GameState GameState { get; private set; } = GameState.Play;

    private PauseScreenController pauseScreen;

    private void Awake()
    {
        pauseScreen = GameObject.Find("PauseScreen").GetComponent<PauseScreenController>();
        pauseScreen.SetGameManager(this);
        Time.timeScale = 1;
    }

    public void SetGameState(GameState newState)
    {
        if (GameState == GameState.Dead)
        {
            return;
        }
        switch (newState)
        {
            case GameState.Play:
                Time.timeScale = 1;
                pauseScreen.Hide();
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                pauseScreen.ShowPause();
                break;
            case GameState.Dead:
                Time.timeScale = 0;
                pauseScreen.ShowDead();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameState = newState;
    }

}