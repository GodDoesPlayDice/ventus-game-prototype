using UnityEngine;

namespace Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        public WeaponData melee;
        public WeaponData ranged;

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
    }
}