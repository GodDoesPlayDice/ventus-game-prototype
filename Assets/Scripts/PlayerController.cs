using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] private float movementSpeed;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _movementInput;

    [SerializeField] private bool isDead = false;

    public void OnMove(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            _movementInput = value.ReadValue<Vector2>();
        }
        else if (value.canceled)
        {
            _movementInput = Vector2.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.performed && !isDead)
        {
        }
    }

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _animator);
    }

    private void Update()
    {
        if (_animator == null || isDead) return;
        if (_movementInput.magnitude >= 0.01f)
        {
            _animator.SetBool("walk", true);
            if (_movementInput.x >= 0.2f)
            {
                spriteRenderer.flipX = false;
            }
            else if (_movementInput.x <= -0.2f)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            _animator.SetBool("walk", false);
        }
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