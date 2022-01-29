using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public interface IActorController
    {
        /**
         * bool param - should remain in _actors list
         */
        public void Act(Action<bool> endTurn);

        public void SetInBattle(bool inBattle);
    }
}