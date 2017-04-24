using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsLocalHost : SceneActions
{
    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(goBackToInit, "Cancel Broadcasting")
            , new TestAction(goOnlineAsHost, "Start Hosting game")
        };
    }

    private void goBackToInit()
    {
        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        SceneManager.LoadScene("SceneInit");
        //StartCoroutine(delayAction(()=>{SceneManager.LoadScene("SceneInit");}, new WaitForSeconds(1f)));
    }

    private IEnumerator delayAction(System.Action _action, WaitForSeconds _delay)
    {
        yield return _delay;

        _action();
    }

    private void goOnlineAsHost()
    {
        SceneActionsOnline.isLocalPlayerHost = true;

        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        // Configure NetworkManager ahead of time.
		SceneActionsOnline.serverNetworkAddress = "localhost";
		SceneActionsOnline.serverNetworkPort = 10000;

		Debug.Log("Starting to host game on port " + SceneActionsOnline.serverNetworkPort);

        SceneManager.LoadScene("SceneOnline");
    }
}
