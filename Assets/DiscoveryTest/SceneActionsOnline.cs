using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsOnline : SceneActions
{
    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(leaveGame, "Leave game")
        };
    }

    private void leaveGame()
    {
        if ( Network.isServer )
        {
            CustomNetworkManager.singleton.StopHost();
        }
        else
        {
            CustomNetworkManager.singleton.StopClient();
        }

        Destroy(CustomNetworkManager.singleton.gameObject, 0.5f);

        SceneManager.LoadScene("SceneInit");
    }
}
