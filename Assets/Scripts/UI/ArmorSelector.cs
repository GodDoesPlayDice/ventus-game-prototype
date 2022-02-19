using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class ArmorSelector : MonoBehaviour
    {
        public LoadSceneEvent loadSceneEvent;
        public SceneEnum sceneToOpen;

        public Image baseLayerOnCharacterView;
        public Image armourLayerOnCharacterView;
        public GameObject startGameButton;

        public TextMeshProUGUI healthValue;
        public TextMeshProUGUI staminaValue;
        public TextMeshProUGUI strengthValue;
        public TextMeshProUGUI luckValue;
        
        private GameObject _selectedArmour;

        private void Start()
        {
            startGameButton.SetActive(false);
        }

        public void SelectArmor(GameObject obj)
        {
            var armourButtonController = obj.GetComponent<ArmourButtonController>();
            var armourSprite = armourButtonController.actualImage;
            
            UpdateCharacterView(armourSprite, armourButtonController.isDisablingBaseLayer);
            UpdateStatsView(armourButtonController);

            _selectedArmour = obj;

            if (!startGameButton.activeSelf)
            {
                startGameButton.SetActive(true);
            }
        }

        public void OnStartGameButtonPressed()
        {
            if (_selectedArmour == null) return;
            SaveSelectedItem(_selectedArmour);

            var sceneEP = new LoadSceneEP(sceneToOpen,
                SceneEnum.TITLE, true, null, null, true);
            loadSceneEvent.Raise(sceneEP);
        }

        private void UpdateCharacterView(Sprite armourSprite, bool isDisablingBaseLayer)
        {
            armourLayerOnCharacterView.sprite = armourSprite;
            baseLayerOnCharacterView.gameObject.SetActive(!isDisablingBaseLayer);
        }

        private void UpdateStatsView(ArmourButtonController armourButtonController)
        {
            healthValue.text = armourButtonController.health;
            staminaValue.text = armourButtonController.stamina;
            strengthValue.text = armourButtonController.strength;
            luckValue.text = armourButtonController.luck;
        }

        private void SaveSelectedItem(GameObject obj)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("ArmorSelection");
            foreach (var it in objs)
            {
                if (it != obj)
                {
                    Destroy(it);
                }
            }

            DontDestroyOnLoad(Instantiate(obj));
        }
    }
}