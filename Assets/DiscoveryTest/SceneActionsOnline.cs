using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsOnline : SceneActions
{
    public static bool isLocalPlayerHost;
	public static string serverNetworkAddress;
	public static int serverNetworkPort;

    [SerializeField] private GameObject interactionPlane;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(leaveGame, "Leave game")
            , new TestAction(spawnStuff, "Spawn stuff")
        };

        if ( isLocalPlayerHost )
        {
            Debug.Log("Starting host");
			NetworkManagerDiscovery.singleton.networkAddress = serverNetworkAddress;
			NetworkManagerDiscovery.singleton.networkPort = serverNetworkPort;
			/*NetworkClient hostClient = */NetworkManagerDiscovery.singleton.StartHost();
        }
        else
        {
            Debug.Log("Starting client");
			NetworkManagerDiscovery.singleton.networkAddress = serverNetworkAddress;
			NetworkManagerDiscovery.singleton.networkPort = serverNetworkPort;
			/*NetworkClient remoteClient = */NetworkManagerDiscovery.singleton.StartClient();
        }
    }

    private void spawnStuff()
    {
        Debug.Log("Spawning");

		// Spawn once only
		sceneActions = new List<TestAction>()
		{
			new TestAction(leaveGame, "Leave game")
		};

		setupActionButtons();

        NetworkServer.SpawnObjects();
    }

    private void leaveGame()
    {
        if ( Network.isServer )
        {
            Debug.Log("Stopping host");
			NetworkManagerDiscovery.singleton.StopHost();
        }
        else
        {
            Debug.Log("Stopping client");
			NetworkManagerDiscovery.singleton.StopClient();
        }

		Debug.Log("NetworkServer.Shutdown");
		NetworkServer.Shutdown();

        SceneManager.LoadScene("SceneInit");
    }
}
