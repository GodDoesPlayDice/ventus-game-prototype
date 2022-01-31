using UnityEngine;

namespace Interactables
{
    public class Money : Bonus
    {
        public float money;

        private bool empty = false;
        
        public override bool Apply(GameObject receiver)
        {
            if (empty)
            {
                return false;
            }

            return true;
        }

        public override string GetPickupText()
        {
            return empty ? "Empty" : $"+ {money} money";
        }
    }
}