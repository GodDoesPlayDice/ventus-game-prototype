using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WalkerController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float movementSpeed;
    private Rigidbody2D _rb;
    private NavMeshAgent _navMeshAgent;
    
    private WalkerController _walkerController;
    private Camera _cam;
    private Vector2 _movementInput;
    private Vector2 _mousePosition;

    [SerializeField] private bool isDead = false;


    public UnityEvent<Vector3> onChangePosition;

    // temp
    private GameObject _pointer;
    
    // animator stuff
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Up = Animator.StringToHash("up");

    public void OnMousePosition(InputAction.CallbackContext value)
    {
        var pos = value.ReadValue<Vector2>();
        var objectPos = _cam.ScreenToWorldPoint(pos);
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
        TryGetComponent(out _navMeshAgent);
        TryGetComponent(out _walkerController);
        if (_cam == null) _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (_pointer == null) _pointer = GameObject.Find("Pointer");
    }

    private void Start()
    {
        onChangePosition ??= new UnityEvent<Vector3>();
        StartCoroutine(UpdatePositionCoroutine());
    }

    private void Update()
    {
        if (animator == null || isDead || _navMeshAgent == null) return;
        
        if (_navMeshAgent.velocity.magnitude >= 0.01f)
        {
            animator.SetBool(Walk, true);
            if (_navMeshAgent.velocity.y >= 0.2f)
            {
                animator.SetBool(Up, true);
            }
            else if (_navMeshAgent.velocity.y <= -0.2f)
            {
                animator.SetBool(Up, false);
            }
            if (_navMeshAgent.velocity.x >= 0.2f)
            {
                spriteRenderer.flipX = false;
            }
            else if (_navMeshAgent.velocity.x <= -0.2f)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool(Walk, false);
        }
    }

    private IEnumerator UpdatePositionCoroutine()
    {
        for (;;)
        {
            if (!isDead);
            onChangePosition.Invoke(transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // private void DirectMovement()
    // {
    //     if (_rb != null && !isDead)
    //         _rb.MovePosition(_rb.position + _movementInput * movementSpeed * Time.fixedDeltaTime);
    // }
}