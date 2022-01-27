using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class WalkerController : MonoBehaviour
{
       private NavMeshAgent _navMeshAgent;

       private void Awake()
       {
              TryGetComponent(out _navMeshAgent);
       }

       private void Start()
       {
           // this is required by the library
           _navMeshAgent.updateRotation = false;
           _navMeshAgent.updateUpAxis = false;
       }

       public void WalkTo(Vector3 position)
       {
           if (_navMeshAgent == null) return;
           _navMeshAgent.destination = position;
       } 
}