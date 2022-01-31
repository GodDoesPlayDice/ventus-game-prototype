using UnityEngine;

namespace Interactables
{
    public class Health : Bonus
    {
        public float health;

        private bool empty = false;
        
        public override bool Apply(GameObject receiver)
        {
            if (empty)
            {
                return false;
            }
                
            receiver.TryGetComponent(out Damageable damageable);
            if (damageable != null)
            {
                damageable.CurrentHealth += health;
            }

            return true;
        }

        public override string GetPickupText()
        {
            return empty ? "Empty" : "+" + health + " health";
        }
    }
}