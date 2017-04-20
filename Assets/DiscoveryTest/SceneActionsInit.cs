using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsInit : SceneActions
{
    [SerializeField] private NetworkManagerDiscovery networkManager;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(startAsHost, "Host game")
            , new TestAction(startAsClient, "Look for games")
            , new TestAction(()=> { Debug.Log("Beepbeep");  }, "Boop")
        };
    }

    private void startAsHost()
    {
        SceneManager.LoadScene("SceneBroadcaster");
    }

    private void startAsClient()
    {
        SceneManager.LoadScene("SceneListener");
    }
}
