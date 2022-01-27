using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class WalkerController : MonoBehaviour
{
       private NavMeshAgent _navMeshAgent;
       private ActorController _actorController;

       private void Awake()
       {
              TryGetComponent(out _navMeshAgent);
              TryGetComponent(out _actorController);
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
           if (_actorController == null || _actorController.actorType != GameManager.CurrentActor) return;
           _navMeshAgent.destination = position;
       } 
}