using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public class BattleManager : MonoBehaviour
    {
        private GameModes mode = GameModes.Peace;
        private List<IActorController> _actors = new List<IActorController>();
        public PlayerController player;
        public UnityEvent<bool> onBattleStatusChange;
        public UnityEvent<IActorController, bool> onBattleActorStatusChange;

        private void Start()
        {
            _actors.Add(player);
        }

        public bool Register(IActorController actor)
        {
            if (_actors.Contains(actor)) return false;
            _actors.Add(actor);
            actor.SetInBattle(true);
            onBattleActorStatusChange.Invoke(actor, true);
            if (mode == GameModes.Peace)
            {
                mode = GameModes.Combat;
                StartBattle();
            }
            return true;
        }

        public bool Unregister(IActorController actor)
        {
            if (!_actors.Contains(actor)) return false; 
            _actors.RemoveAt(_actors.IndexOf(actor));
            actor.SetInBattle(false);
            onBattleActorStatusChange.Invoke(actor, false);
            if (_actors.Count <= 1)
            {
                mode = GameModes.Peace;
                StopBattle();
            }
            return true;
        }

        private void StartBattle()
        {
            onBattleStatusChange.Invoke(true);
            player.SetInBattle(true);
            NextTurn();
            Debug.Log("Battle");
        }

        private void StopBattle()
        {
            onBattleStatusChange.Invoke(false);
            player.SetInBattle(false);
        }

        private void NextTurn()
        {
            _actors[0].Act(succeed =>
            {
                var tmp = _actors[0];
                _actors.RemoveAt(0);
                if (succeed)
                {
                    _actors.Add(tmp);
                }

                NextTurn();
            });
        }
    }
}