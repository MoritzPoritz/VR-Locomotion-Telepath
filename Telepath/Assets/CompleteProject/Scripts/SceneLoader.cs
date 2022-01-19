using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    private List<int> scenesToLoad = new List<int>();

    [SerializeField]
    private int questionnaireScene;

    private static SceneLoader instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            instance.Start();

            // Might be unneccessary
            Destroy(this);
        }
    }

    private void Start()
    {
        if (this == instance)
        {
            if (scenesToLoad.Count > 0)
            {
                int sceneToLoadIndex = Random.Range(0, scenesToLoad.Count);
                int sceneToLoad = scenesToLoad[sceneToLoadIndex];
                scenesToLoad.RemoveAt(sceneToLoadIndex);
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
            else
            {
                SceneManager.LoadSceneAsync(questionnaireScene);
            }
        }
    }




}
