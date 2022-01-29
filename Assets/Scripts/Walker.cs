using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


[RequireComponent(typeof(NavMeshAgent))]
public class Walker : MonoBehaviour
{
    public Vector3 destination;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _prevPos;

    private void Awake()
    {
        TryGetComponent(out _navMeshAgent);
    }

    private void Start()
    {
        // this is required by the library
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _prevPos = transform.position;
    }

    public void Walk()
    {
        Walk(destination);
    }

    public void Walk(Vector3 position)
    {
        if (_navMeshAgent == null) return;
        _navMeshAgent.destination = position;
    }

    public void Stop()
    {
        _navMeshAgent.isStopped = true;
    }

    public float GetDeltaDistance()
    {
        var position = transform.position;

        var distance = Vector3.Distance(position, _prevPos);
        _prevPos = position;
        return distance;
    }

    public bool IsReachedDestination()
    {
        bool result = false;
        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0.5f)
                {
                    result = true;
                }
            }
        }
        return result;
    }
}