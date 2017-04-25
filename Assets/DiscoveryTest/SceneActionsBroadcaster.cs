using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsBroadcaster : SceneActions
{
	[SerializeField] private TestNetServer discoveryBroadcaster;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(goBackToInit, "Cancel Broadcasting")
            , new TestAction(goOnlineAsHost, "Start Hosting game")
        };
    }

	protected override void Start()
	{
		base.Start();

		discoveryBroadcaster.startServer();
	}

    private void goBackToInit()
    {
        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        endBroadcasting();

        SceneManager.LoadScene("SceneInit");
    }

    private void endBroadcasting()
    {
        TestNetServer broadcaster = GameObject.FindObjectOfType<TestNetServer>();
        broadcaster.StopBroadcast();
        Destroy(broadcaster.gameObject);

        NetworkManagerDiscovery.singleton.StopHost();
    }

    private void goOnlineAsHost()
    {
        SceneActionsOnline.isLocalPlayerHost = true;

        sceneActions = new List<TestAction>(0);
        setupActionButtons();

        // Configure NetworkManager ahead of time.
		SceneActionsOnline.serverNetworkAddress = Network.player.ipAddress;
		SceneActionsOnline.serverNetworkPort = TestNetServer.serverPortUsed;

		Debug.Log("Starting to host game on port " + SceneActionsOnline.serverNetworkPort);

        endBroadcasting();

        SceneManager.LoadScene("SceneOnline");
    }
}
