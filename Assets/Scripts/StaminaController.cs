using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaminaController : MonoBehaviour
{
    public float maxStamina = 10;

    public float _stamina;
    
    public UnityEvent<float, float> onCurrentStaminaChange;

    public void ResetStamina()
    {
        _stamina = maxStamina;
        onCurrentStaminaChange.Invoke(_stamina, maxStamina);
    }

    public bool UseStaminaIfEnough(float cost)
    {
        if (_stamina >= cost)
        {
            _stamina -= cost;
            onCurrentStaminaChange.Invoke(_stamina, maxStamina);
            return true;
        }

        return false;
    }
    
    // private bool IsEnough(float cost)
    // {
    //     return _stamina >= cost;
    // }
}
