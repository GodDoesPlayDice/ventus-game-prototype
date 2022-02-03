using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Weapon", order = 0)]
    public class WeaponData : ScriptableObject
    {
        public WeaponType type;
        public Sprite sprite;
        public bool melee;
        public float damage = 20;
        public float distance;
        public float staminaCost = 3;
        public string prefix;
    }
}