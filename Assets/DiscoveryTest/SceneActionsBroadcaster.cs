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
        sceneActions = new List<TestAction>(0);

        setupActionButtons();

        TestNetServer broadcaster = GameObject.FindObjectOfType<TestNetServer>();
        broadcaster.StopBroadcast();
        Destroy(broadcaster.gameObject);

        NetworkManagerDiscovery.singleton.StopHost();

        SceneManager.LoadScene("SceneInit");
        //StartCoroutine(delayAction(()=>{SceneManager.LoadScene("SceneInit");}, new WaitForSeconds(1f)));
    }

    private IEnumerator delayAction(System.Action _action, WaitForSeconds _delay)
    {
        yield return _delay;

        _action();
    }

    private void startHostingGame()
    {
        SceneManager.LoadScene("SceneOnline");
    }
}
