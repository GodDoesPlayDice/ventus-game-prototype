using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Enums;

public class HudController : MonoBehaviour
{
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