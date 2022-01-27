using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(WalkerController))]
public class PlayerController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] private float movementSpeed;
    private Rigidbody2D _rb;
    private Animator _animator;
    private WalkerController _walkerController;
    private Camera _cam;
    private Vector2 _movementInput;
    private Vector2 _mousePosition;

    [SerializeField] private bool isDead = false;


    // temp
    private GameObject _pointer;

    public void OnMousePosition(InputAction.CallbackContext value)
    {
        Vector2 pos = value.ReadValue<Vector2>();
        Vector3 objectPos = _cam.ScreenToWorldPoint(pos);
        _pointer.transform.position = new Vector3(objectPos.x, objectPos.y, 0f);
        _mousePosition = pos;
    }

    public void OnPointerClick(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            _walkerController.WalkTo(_pointer.transform.position);
        }
    }

    // public void OnMove(InputAction.CallbackContext value)
    // {
    //     if (value.performed)
    //     {
    //         _movementInput = value.ReadValue<Vector2>();
    //     }
    //     else if (value.canceled)
    //     {
    //         _movementInput = Vector2.zero;
    //     }
    // }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.performed && !isDead)
        {
        }
    }

    private void PointerLogic(Vector2 mousePosition)
    {
        if (_cam == null) return;
        Vector2 v = _cam.ScreenToWorldPoint(mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(v, Vector2.zero);
        if (hit)
        {
            Debug.Log(" mouse over: " + hit.collider.gameObject.name);
        }
    }

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _animator);
        TryGetComponent(out _walkerController);
        if (_cam == null) _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (_pointer == null) _pointer = GameObject.Find("Pointer");
    }

    private void Update()
    {
        // if (_animator == null || isDead) return;
        // if (_movementInput.magnitude >= 0.01f)
        // {
        //     _animator.SetBool("walk", true);
        //     if (_movementInput.x >= 0.2f)
        //     {
        //         spriteRenderer.flipX = false;
        //     }
        //     else if (_movementInput.x <= -0.2f)
        //     {
        //         spriteRenderer.flipX = true;
        //     }
        // }
        // else
        // {
        //     _animator.SetBool("walk", false);
        // }
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_rb != null && !isDead)
            _rb.MovePosition(_rb.position + _movementInput * movementSpeed * Time.fixedDeltaTime);
    }
}