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
            new TestAction(goBackToInit, "Cancel Broadcasting")
            , new TestAction(goOnlineAsHost, "Start Hosting game")
        };
    }

    private void goBackToInit()
    {
        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        endBroadcasting();

        SceneManager.LoadScene("SceneInit");
        //StartCoroutine(delayAction(()=>{SceneManager.LoadScene("SceneInit");}, new WaitForSeconds(1f)));
    }

    private void endBroadcasting()
    {
        TestNetServer broadcaster = GameObject.FindObjectOfType<TestNetServer>();
        broadcaster.StopBroadcast();
        Destroy(broadcaster.gameObject);

        NetworkManagerDiscovery.singleton.StopHost();
    }

    private IEnumerator delayAction(System.Action _action, WaitForSeconds _delay)
    {
        yield return _delay;

        _action();
    }

    private void goOnlineAsHost()
    {
        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        endBroadcasting();

        SceneManager.LoadScene("SceneOnline");
    }
}
