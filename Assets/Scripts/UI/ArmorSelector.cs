using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ArmorSelector : MonoBehaviour
    {
        public LoadSceneEvent loadSceneEvent;
        public SceneEnum sceneToOpen;

        public void SelectArmor(GameObject obj)
        {
            SaveSelectedItem(obj);
            
            var sceneEP = new LoadSceneEP(sceneToOpen,
                SceneEnum.TITLE, true, null, null, true);
            loadSceneEvent.Raise(sceneEP);
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