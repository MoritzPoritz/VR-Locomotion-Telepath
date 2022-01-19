using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{



    public delegate void OnSimpleSceneLoaded(Transform startingPos);
    public static event OnSimpleSceneLoaded SimpleSceneLoadedEvent;

    public static void CallSimpleSceneLoaded(Transform startingPos)
    {
        Debug.Log("Was called");
        SimpleSceneLoadedEvent.Invoke(startingPos);
    }

    public delegate void OnLastCheckpointReached(string sceneName);
    public static event OnLastCheckpointReached LastCheckpointReached;

    public static void InvokeLastCheckpointReached(string sceneName)
    {
        LastCheckpointReached.Invoke(sceneName);
    }
}
