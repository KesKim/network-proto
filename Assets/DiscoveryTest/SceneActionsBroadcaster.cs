using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsBroadcaster : SceneActions
{
    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(cancelBroadcasting, "Cancel Broadcasting")
            , new TestAction(startHostingGame, "Start Hosting game")
        };
    }

    private void cancelBroadcasting()
    {
        SceneManager.LoadScene("SceneInit");
    }

    private void startHostingGame()
    {
        SceneManager.LoadScene("SceneOnline");
    }
}
