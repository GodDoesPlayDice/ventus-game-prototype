using System;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [HideInInspector] public Vector3 mousePosition;
    private void Update()
    {
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
    }
}