using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsInit : SceneActions
{
    [SerializeField] private NetworkManagerDiscovery networkManager;

    private void Awake()
    {
		Application.runInBackground = true;

        sceneActions = new List<TestAction>()
        {
			//new TestAction(startAsLocalHost, "Host Local")
			//, new TestAction(startAsLocalClient, "Join Local")
			new TestAction(hostLobby, "Host Lobby")
			, new TestAction(joinLobby, "Join Lobby")
			, new TestAction(toPlayerOptions, "Player Options")
        };

		if ( NetworkManagerDiscovery.singleton == null )
		{
			networkManager.resetPlayerIds();
		}
		else
		{
			((NetworkManagerDiscovery)NetworkManagerDiscovery.singleton).resetPlayerIds();
		}
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

	private void hostLobby()
	{
		SceneActionsLobby.enterAsHost = true;
		SceneManager.LoadScene("SceneSharedLobby");
	}

	private void joinLobby()
	{
		SceneActionsLobby.enterAsHost = false;
		SceneManager.LoadScene("SceneSharedLobby");
	}

	private void toPlayerOptions()
	{
		SceneManager.LoadScene("ScenePlayerOptions");
	}
}
