using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsLobby : SceneActions
{
	public static bool enterAsHost;

	[SerializeField] private TestNetServer discoveryBroadcaster;
	[SerializeField] private TestNetClient discoveryListener;

	protected override void Start()
	{
		sceneActions = new List<TestAction>()
		{
			new TestAction(()=>{Debug.Log("foo");}, "foo")
		};

		base.Start();

		if ( enterAsHost )
		{
			// Defines port used
			discoveryBroadcaster.startServer();

			Debug.Log("Starting host");
			string serverNetworkAddress = Network.player.ipAddress;
			int gameServerNetworkPort = TestNetServer.serverPortUsed + 1;

			NetworkManagerDiscovery.singleton.networkAddress = serverNetworkAddress;
			NetworkManagerDiscovery.singleton.networkPort = gameServerNetworkPort;
			/*NetworkClient hostClient = */NetworkManagerDiscovery.singleton.StartHost();
		}
		else
		{
			TestNetClient.serverFoundEvent -= onServerFound;
			TestNetClient.serverFoundEvent += onServerFound;

			discoveryListener.startClient();
		}
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

	#region Host
	private void endBroadcasting()
	{
		discoveryBroadcaster.StopBroadcast();
		Destroy(discoveryBroadcaster.gameObject);
		
		NetworkManagerDiscovery.singleton.StopHost();
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
	#endregion

	#region Remote Client
	private void endListening()
	{
		TestNetClient.serverFoundEvent -= onServerFound;
		
		discoveryListener.StopBroadcast();
		Destroy(discoveryListener.gameObject);
	
	}
	private void onServerFound(string _fromAddress, string _data)
	{
		//Debug.Log(TestNetClient.Instance.broadcastsReceived == null ? "Null broadcast Dict." : TestNetClient.Instance.broadcastsReceived.Count < 1 ? "Empty broadcast Dict." : TestNetClient.Instance.broadcastsReceived.Count + " entries known.");
		
		if ( discoveryListener.broadcastsReceived != null && discoveryListener.broadcastsReceived.Count > 0 )
		{
			int count = discoveryListener.broadcastsReceived.Count;
			sceneActions = new List<TestAction>(count + 1);
			sceneActions.Add(new TestAction(listenerBackToInit, "Stop listening"));
			
			foreach ( KeyValuePair<string, NetworkBroadcastResult> kvp in discoveryListener.broadcastsReceived )
			{
				sceneActions.Add(new TestAction(()=>{joinGame(kvp.Value.serverAddress);}, kvp.Value.serverAddress));
			}
			
			setupActionButtons();
		}
		else
		{
			sceneActions = new List<TestAction>()
			{
				new TestAction(listenerBackToInit, "Stop listening")
			};
		}
	}

	private void joinGame(string _serverAddress)
	{
		SceneActionsOnline.isLocalPlayerHost = false;

		TestNetClient listener = GameObject.FindObjectOfType<TestNetClient>();
		Destroy(listener.gameObject);

		string portDataString = BytesToString(TestNetClient.Instance.broadcastsReceived[_serverAddress].broadcastData);
		int port = System.Convert.ToInt32(portDataString);

		Debug.Log("Joining as client to " + _serverAddress + " on port " + port);

		if ( NetworkManager.singleton != null && NetworkManager.singleton.client == null )
		{
			SceneActionsOnline.serverNetworkAddress = _serverAddress;
			SceneActionsOnline.serverNetworkPort = port;

			endListening();
		}
	}

	private void listenerBackToInit()
	{
		endListening();

		SceneManager.LoadScene("SceneInit");
	}

	static string BytesToString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}
	#endregion
}
