using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitchingPoint : MonoBehaviour
{
    public LoadSceneEvent loadSceneEvent;
    public SceneEnum scene;

    public void SwitchScene()
    {
        var loadSceneEp = new LoadSceneEP(scene,
            (SceneEnum) SceneManager.GetActiveScene().buildIndex, true,
            null, null, true);
        loadSceneEvent.Raise(loadSceneEp);
    }
}