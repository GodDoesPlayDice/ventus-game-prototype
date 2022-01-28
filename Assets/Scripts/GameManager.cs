using System;
using System.Collections;
using UnityEngine;
using Enums;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameState GameState { get; private set; } = GameState.Play;

    public static void SetGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Play:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameState = newState;
    }

}