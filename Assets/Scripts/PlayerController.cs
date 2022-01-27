using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Enums;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WalkerController))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    private NavMeshAgent _navMeshAgent;
    private Damageable _damageable;
    private ActorController _actorController;

    private Camera _cam;
    private Vector2 _mousePosition;
    private GameObject _pointer;

    public UnityEvent<Vector3> onChangePosition;

    // animator stuff
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Up = Animator.StringToHash("up");

    public void OnMousePosition(InputAction.CallbackContext value)
    {
        var pos = value.ReadValue<Vector2>();
        var objectPos = _cam.ScreenToWorldPoint(pos);
        _mousePosition = pos;
        _pointer.transform.position = new Vector3(objectPos.x, objectPos.y, 0f);
    }

    public void OnPointerClick(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            PointerClickLogic();
        }
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.performed && !_damageable.IsDead)
        {
        }
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (GameManager.GameState != GameState.Pause)
            {
                GameManager.SetGameState(GameState.Pause);
            }
            else
            {
                GameManager.SetGameState(GameState.Play);
            }
        }
    }

    private void PointerClickLogic()
    {
        if (_cam == null) return;
        Vector2 v = _cam.ScreenToWorldPoint(_mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(v, Vector2.zero);
        var objectTag = hit.collider.gameObject.tag;
        if (objectTag == "Enemy")
        {
            if (_actorController.animator == null) _actorController.animator = animator;
            _actorController.Act(GameActions.Attack);
            return;
        }
        if (objectTag == "Ground")
        {
            _actorController.destination = _pointer.transform.position;
            _actorController.Act(GameActions.Move);
            return;
        }
    }

    private void Awake()
    {
        TryGetComponent(out _navMeshAgent);
        TryGetComponent(out _damageable);
        TryGetComponent(out _actorController);
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
        if (animator == null || _damageable.IsDead || _navMeshAgent == null) return;

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
            if (!_damageable.IsDead)
            {
                onChangePosition.Invoke(transform.position);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}