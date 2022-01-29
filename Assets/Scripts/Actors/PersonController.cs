using Actions;
using Enums;
using Interfaces;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;


public class PersonController : MonoBehaviour
{
    private enum ActionStatus
    {
        InProgress, StaminaOut, Completed
    }
    
    public float moveStaminaCost = 0.2f;
    public float attackStaminaCost = 3f;
    public float distanceToAttack = 2f;
    public float maxStamina = 10;
    
    public UnityEvent<float, float> onCurrentStaminaChange;

    //public float attackTime = 1f;
    
    public float _stamina;
    private bool _ignoreStamina = true;
    private ActorAction _action;
    
    private Walker _walker;
    private Attacker _attacker;
    //private IActorAnimationManager _animationManager;

    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _attacker);
        //TryGetComponent(out _animationManager);
    }
    
    private void Start()
    {
        onCurrentStaminaChange ??= new UnityEvent<float, float>();
        ResetStamina();
    }

    public void ResetStamina()
    {
        _stamina = maxStamina;
        onCurrentStaminaChange.Invoke(_stamina, maxStamina);
    }

    public void SetIgnoreStamina(bool ignoreStamina)
    {
        _ignoreStamina = ignoreStamina;
    }

    public void SetAction(ActorAction action)
    {
        Debug.Log("SetAction " + action);
        _action = action;
        
        // Clear delta distance
        _walker.GetDeltaDistance();
    }
    
    void Update()
    {
        if (_action != null)
        {
            Act();
        }
    }

    private void Act()
    {
        ActionStatus status = ActionStatus.InProgress;
        if (_action.type == ActionType.Move)
        {
            status = Move();
        } else if (_action.type == ActionType.Attack)
        {
            status = Attack();
        }

        if (!_ignoreStamina)
        {
            onCurrentStaminaChange.Invoke(_stamina, maxStamina);
        }

        var tmpAction = _action;
        if (status == ActionStatus.Completed)
        {
            _action = null;
            Debug.Log(status);
            tmpAction.completeCallback(true);
        }
        else if (status == ActionStatus.StaminaOut)
        {
            _action = null;
            Debug.Log(status);
            tmpAction.completeCallback(false);
        }
    }

    private ActionStatus Move()
    {
        var status = ActionStatus.InProgress;

        var distance = _walker.GetDeltaDistance();
        _stamina -= distance * moveStaminaCost;
        if (_ignoreStamina || _stamina > 0)
        {
            if (!_action.started)
            {
                _action.MarkStarted();
                _walker.Walk(_action.position);
            }
            else if (_walker.IsReachedDestination())
            {
                status = ActionStatus.Completed;
            }
        }
        else
        {
            _walker.Stop();
            status = ActionStatus.StaminaOut;
        }
        
        return status;
    }

    private ActionStatus Attack()
    {
        var status = ActionStatus.InProgress;
        if (Vector3.Distance(transform.position, _action.target.transform.position) > distanceToAttack)
        {
            status = Move();
        }
        else
        {
            _walker.Stop();
            if (_ignoreStamina || _stamina - attackStaminaCost > 0)
            {
                // TODO: wait animation to play !!!
                
                _attacker.Attack(_action.target);
                status = ActionStatus.Completed;
                _stamina -= attackStaminaCost;
            }
            else
            {
                status = ActionStatus.StaminaOut;
            }
        }
        return status;
    }

    // TODO: Interact
    private void Interact()
    {
        
    }

    // TODO: Turn
    private void Turn()
    {
        
    }
}
