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
			, new TestAction(startAsLocalHost, "Host as local")
			, new TestAction(startAsLocalClient, "Join as local")
        };

		networkManager.resetPlayerIds();
    }

	protected override void Start()
	{
		base.Start();

		int networkDropTreshold = 50;
		NetworkManagerDiscovery.singleton.connectionConfig.NetworkDropThreshold = (byte)networkDropTreshold;

		int overflowDropTreshold = 50;
		NetworkManagerDiscovery.singleton.connectionConfig.OverflowDropThreshold = (byte)overflowDropTreshold;
	}

    private void startAsHost()
    {
        SceneManager.LoadScene("SceneBroadcaster");
    }

    private void startAsClient()
    {
        SceneManager.LoadScene("SceneListener");
    }

	private void startAsLocalHost()
	{
		SceneManager.LoadScene("SceneLocalHost");
	}

	private void startAsLocalClient()
	{
		SceneManager.LoadScene("SceneLocalClient");
	}

}
