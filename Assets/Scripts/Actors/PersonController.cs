using Actions;
using Enums;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Weapons;


public class PersonController : MonoBehaviour
{
    private enum ActionStatus
    {
        InProgress, StaminaOut, Completed
    }
    
    public float moveStaminaCost = 0.2f;
    public float attackStaminaCost = 3f;
    //public float distanceToAttack = 2f;
    public float distanceToInteract = 2f;
    // public float maxStamina = 10;
    
    // public UnityEvent<float, float> onCurrentStaminaChange;

    //public float attackTime = 1f;
    
    // public float _stamina;
    private bool _ignoreStamina = true;
    private ActorAction _action;
    
    private Walker _walker;
    private Attacker _attacker;

    
    public float attackDelay = 1f;
    private float _attackStartTime;
    private IActorAnimationManager _animationManager;
    private WeaponHolder _weaponHolder;
    private StaminaController _staminaController;

    private bool stopped;
    

    private void Awake()
    {
        TryGetComponent(out _walker);
        TryGetComponent(out _attacker);
        _staminaController = GetComponent<StaminaController>();
        //TryGetComponent(out _animationManager);
        _animationManager = GetComponent<IActorAnimationManager>();
        _weaponHolder = GetComponent<WeaponHolder>();
    }
    
    private void Start()
    {
        // onCurrentStaminaChange ??= new UnityEvent<float, float>();
        ResetStamina();
    }

    public void ResetStamina()
    {
        // _stamina = maxStamina;
        _staminaController.ResetStamina();
        // onCurrentStaminaChange.Invoke(_staminaController._stamina, _staminaController.maxStamina);
    }

    public void SetIgnoreStamina(bool ignoreStamina)
    {
        _ignoreStamina = ignoreStamina;
    }

    public void SetAction(ActorAction action)
    {
        _action = action;
        
        // Clear delta distance
        _walker.GetDeltaDistance();
    }
    
    void Update()
    {
        if (_action != null)
        {
            stopped = false;
            Act();
        }
        else if (!stopped)
        {
            stopped = true;
            _walker.Stop();
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
        } else if (_action.type == ActionType.Interact)
        {
            status = Interact();
            // Debug.Log("Interact!!!!! " + status);
        }

        // if (!_ignoreStamina)
        // {
        //     onCurrentStaminaChange.Invoke(_staminaController._stamina, _staminaController.maxStamina);
        // }

        var tmpAction = _action;
        if (status == ActionStatus.Completed)
        {
            _action = null;
            tmpAction.completeCallback(true);
        }
        else if (status == ActionStatus.StaminaOut)
        {
            _action = null;
            tmpAction.completeCallback(false);
        }
    }

    private ActionStatus Move()
    {
        var status = ActionStatus.InProgress;

        var distance = _walker.GetDeltaDistance();
        // _stamina -= distance * moveStaminaCost;
        if (_ignoreStamina || _staminaController.UseStaminaIfEnough(distance * moveStaminaCost))
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
        var distance = Vector3.Distance(transform.position, _action.target.transform.position);
        if (_attackStartTime + attackDelay > Time.time)
        {
            // Nothing, InProgress. Should listen animation instead?
        }
        else if (distance > _weaponHolder.GetMaxDistance())
        {
            status = Move();
        }
        else if (_action.target.IsDead)
        {
            // Do nothing
        }
        else
        {
            _walker.Stop();
            if (_ignoreStamina || _staminaController.UseStaminaIfEnough(attackStaminaCost))
            {
                // TODO: wait animation to play !!!
                
                _attacker.Attack(_action.target, _weaponHolder.GetWeaponForDistance(distance));
                status = ActionStatus.Completed;
                // _stamina -= attackStaminaCost;
                _attackStartTime = Time.time;
            }
            else
            {
                status = ActionStatus.StaminaOut;
            }
        }
        return status;
    }
    
    private ActionStatus Interact()
    {
        var status = ActionStatus.InProgress;

        if (_attackStartTime + attackDelay > Time.time)
        {
            // Nothing, InProgress. Should listen animation instead?
        }
        else if (Vector3.Distance(transform.position,_action.interactable.GetClosestPoint(gameObject)) > distanceToInteract)
        {
            status = Move();
        }
        else
        {
            _walker.Stop();
            if (_ignoreStamina || _staminaController.UseStaminaIfEnough(attackStaminaCost))
            {
                // Add Interactor if needed
                
                _action.interactable.Interact(gameObject);
                status = ActionStatus.Completed;
                // _stamina -= attackStaminaCost;
                _attackStartTime = Time.time;
            }
            else
            {
                status = ActionStatus.StaminaOut;
            }
        }

        return status;
    }

    // TODO: Turn
    private void Turn()
    {
        
    }

    public void Die()
    {
        _animationManager.DeathAnimation();
    }
}
