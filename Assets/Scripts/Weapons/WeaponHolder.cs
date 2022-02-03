using System;
using UnityEngine;

namespace Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        [field: SerializeField] public WeaponData melee { get; private set; }
        [field: SerializeField] public WeaponData ranged { get; private set; }

        public float changeStaminaCost = 1;

        private StaminaController _staminaController;

        private void Awake()
        {
            _staminaController = GetComponent<StaminaController>();
        }

        // Ranged must have more distnce then melee
        public float GetMaxDistance()
        {
            if (ranged != null)
            {
                return ranged.distance;
            }

            return melee.distance;
        }

        public WeaponData GetWeaponForDistance(float distance)
        {
            WeaponData result = null;
            if (melee.distance >= distance)
            {
                result = melee;
            } else if (ranged.distance >= distance)
            {
                result = ranged;
            }

            return result;
        }

        public bool ChangeRanged(WeaponData weapon)
        {
            if (_staminaController.UseStaminaIfEnough(changeStaminaCost))
            {
                ranged = weapon;
                return true;
            }

            return false;
        }
    }
}