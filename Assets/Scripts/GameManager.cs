using System;
using System.Collections;
using Actors;
using UnityEngine;
using Enums;
using UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameState GameState { get; private set; } = GameState.Play;
    public LoadSceneEvent loadSceneEvent;

    private Damageable _playerDamageable;
    private PauseScreenController _pauseScreen;

    private void Awake()
    {
        _playerDamageable = GameObject.Find("Player").GetComponent<Damageable>();
        
        _pauseScreen = GameObject.Find("PauseScreen").GetComponent<PauseScreenController>();
        _pauseScreen.SetGameManager(this);
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
                _pauseScreen.Hide();
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                _pauseScreen.ShowPause();
                break;
            case GameState.Dead:
                Time.timeScale = 0;
                _pauseScreen.ShowDead();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        GameState = newState;
    }


    public void SetPlayerHp(string param)
    {
        Debug.Log("Health: " + param);
        var parsed = float.TryParse(param, out float hp);
        if (parsed)
        {
            _playerDamageable.CurrentHealth = hp;
        }
    }
    
    public void SwitchScene(SceneEnum scene)
    {
        var loadSceneEp = new LoadSceneEP(scene,
            (SceneEnum) SceneManager.GetActiveScene().buildIndex, true,
            null, _playerDamageable.CurrentHealth.ToString(), true);
        loadSceneEvent.Raise(loadSceneEp);
    }
}