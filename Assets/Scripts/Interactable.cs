using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float textTime = 1f;
    public float textMovement = 0.1f;
    
    private Bonus _bonus;
    private float _pickupTime;
    
    private Vector2 _currentTextPosition;
    private GUIStyle _textStyle;
    private Rect _textRect;
    private string _pickupText;

    private Camera camera;

    private void Awake()
    {
        TryGetComponent(out _bonus);
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public bool Interact(GameObject receiver)
    {
        Debug.Log("Interact " + receiver.name);
        bool result = false;
        _pickupText = "Empty";
        if (_bonus != null)
        {
            _bonus.Apply(receiver);
            _pickupText = _bonus.GetPickupText();
            result = true;
        }

        PrepareTextProperties();
        _pickupTime = Time.time;
        return result;
    }

    public Vector2 GetClosestPoint(GameObject receiver)
    {
        var collider = GetComponent <BoxCollider2D>();
        return collider.ClosestPoint(receiver.transform.position);
    }
    
    private void PrepareTextProperties()
    {
        int w = Screen.width, h = Screen.height;
        _textStyle = new GUIStyle();
        _textStyle.alignment = TextAnchor.UpperCenter;
        _textStyle.fontSize = h * 4 / 100;
        _textStyle.normal.textColor = Color.green;
        //_textStyle.font = textFont;
        _textRect = new Rect(0, 0, w / 3, h * 2 / 100);
        _currentTextPosition = 
            camera.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y - 5));
        
    }
    
    private void OnGUI()
    {
        if (_pickupTime == 0f)
        {
            return;
        }

        transform.localScale = Vector3.zero;  // Or disable? Or disable MeshRenderer?
        transform.GetComponent<Collider2D>().enabled = false;

        if (Time.time < _pickupTime + textTime)
        {
            _currentTextPosition.y -= Time.deltaTime / textTime * textMovement * Screen.height;
            _textRect.center = _currentTextPosition;
            GUI.Label(_textRect, _pickupText, _textStyle);
        }
        else
        {
            _pickupTime = 0f;
            DestroyPickedUp();
        }
    }
    
    protected void DestroyPickedUp()
    {
        Destroy(gameObject);
    }
}
