using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitchingPoint : MonoBehaviour
{
    public SceneEnum scene;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SwitchScene()
    {
        _gameManager.SwitchScene(scene);
    }
}