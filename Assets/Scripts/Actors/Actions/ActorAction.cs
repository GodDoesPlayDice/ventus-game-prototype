using System;
using Enums;
using UnityEngine;

namespace Actions
{
    public class ActorAction
    {
        public Action<bool> completeCallback { get; private set; }
        public ActionType type { get; private set; }
        public Vector3 position { get; private set; }
        public Damageable target { get; private set; }

        public bool started { get; private set; } = false;

        // Position should be filled properly!
        private ActorAction(ActionType type, Vector3 position, Damageable target, Action<bool> complete)
        {
            this.type = type;
            this.position = position;
            this.target = target;
            this.completeCallback = complete;
        }

        public void MarkStarted()
        {
            this.started = true;
        }

        
        
        public static ActorAction Move(Vector3 position, Action<bool> complete)
        {
            return new ActorAction(ActionType.Move, position, null, complete);
        }

        public static ActorAction Attack(Damageable target, Action<bool> complete)
        {
            return new ActorAction(ActionType.Attack, target.transform.position, target, complete);
        }

        // TODO: Interact, turn
    }
}