using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
            Debug.Log("Stopping host");
            CustomNetworkManager.singleton.StopHost();
        }
        else
        {
            Debug.Log("Stopping client");
            CustomNetworkManager.singleton.StopClient();
        }

//        Destroy(CustomNetworkManager.singleton.gameObject, 0.5f);

        SceneManager.LoadScene("SceneInit");
    }
}
