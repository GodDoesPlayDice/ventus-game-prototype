using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    public LoadSceneEvent loadSceneEvent;

    public void LoadScene(SceneEnumHolder sceneHolder)
    {
        var sceneParam = new LoadSceneEP(sceneHolder.scene, SceneEnum.TITLE,
            true, null, null, true);
        loadSceneEvent.Raise(sceneParam);
    }
}