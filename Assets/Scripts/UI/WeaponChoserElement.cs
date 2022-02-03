using System;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    public class WeaponChoserElement : MonoBehaviour
    {
        public Action<WeaponData> clickHandler;
        private WeaponData _weapon;

        public void SetWeaponData(WeaponData weaponData)
        {
            this._weapon = weaponData;
            GetComponent<Image>().sprite = _weapon.sprite;
        }

        public void Onclick()
        {
            clickHandler?.Invoke(_weapon);
        }
    }
}