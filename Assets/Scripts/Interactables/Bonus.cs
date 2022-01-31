using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    public abstract bool Apply(GameObject receiver);

    public abstract string GetPickupText();
}
