using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsInit : SceneActions
{
    [SerializeField] private Object broadcasterScene;
    [SerializeField] private Object listenerScene;

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
        Debug.Log("startAsHost");
        int sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/DiscoveryTest/" + broadcasterScene.name + ".unity");
        SceneManager.LoadScene(sceneBuildIndex);
    }

    private void startAsClient()
    {
        Debug.Log("startAsClient");
        int sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/DiscoveryTest/" + listenerScene.name + ".unity");
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
