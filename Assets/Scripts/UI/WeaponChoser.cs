using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    public class WeaponChoser : MonoBehaviour
    {
        public GameObject weaponButtonPrefab;
        public GameObject container;
        public WeaponData[] weapons;
        public float itemsMargin = 10;
        
        private List<WeaponChoserElement> _elements = new List<WeaponChoserElement>();
        private Image _image;

        private WeaponHolder _playerWeapons;

        private void Start()
        {
            _playerWeapons = GameObject.Find("Player").GetComponentInChildren<WeaponHolder>();
            _image = GetComponent<Image>();
            _image.sprite = _playerWeapons.ranged.sprite;
            
            BuildList();
        }

        private void BuildList()
        {
            var pos = transform.position;
            var offset = new Vector3(0,
                weaponButtonPrefab.GetComponentInChildren<Image>().rectTransform.rect.height / 2 + itemsMargin);
            var prevPosition = pos;
            foreach (var weapon in weapons)
            {
                var item = Instantiate(weaponButtonPrefab, prevPosition + offset,
                    Quaternion.identity, container.transform).GetComponentInChildren<WeaponChoserElement>();
                // item.SetActive(false);
                item.clickHandler = OnItemSelected;
                item.SetWeaponData(weapon);
                prevPosition = item.transform.position;
                _elements.Add(item);
            }
        }

        private void OnItemSelected(WeaponData weapon)
        {
            _playerWeapons.ranged = weapon;
            _image.sprite = weapon.sprite;
            HideWeapons();
        }

        public void SwitchActive()
        {
            if (container.activeSelf)
            {
                HideWeapons();
            }
            else
            {
                ShowWeapons();
            }
        }
        
        private void ShowWeapons()
        {
            container.SetActive(true);
        }

        private void HideWeapons()
        {
            container.SetActive(false);
        }
    }
}