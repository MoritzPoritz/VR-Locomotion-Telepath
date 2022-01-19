using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "ScriptableObjects/SceneList", order = 1)]

public class SceneList : ScriptableObject
{
    [SerializeField]
    private List<string> _scenesToLoad;

    
    
    
    
    



    public string GetNextScene()
    {
        var nextScene = "StartScene";
        if (_scenesToLoad.Count > 0)
        {
            int sceneIndex = Random.Range(0, _scenesToLoad.Count);
            nextScene = _scenesToLoad[sceneIndex];
        }
        else
        {
            nextScene = "StartScene";

        }

        _scenesToLoad.Remove(nextScene);
        return nextScene;
    }
}
