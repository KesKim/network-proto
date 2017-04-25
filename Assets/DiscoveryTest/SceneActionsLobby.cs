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

	private int gameServerNetworkPort;

	private List<TestAction> noActions;
	private List<TestAction> broadcasterStatic;
	private List<TestAction> listenerStatic;
	private List<TestAction> listenerPostJoin;

	private void Awake()
	{
		noActions = new List<TestAction>(){};
		broadcasterStatic = new List<TestAction>(){new TestAction(leaveGame, "Leave game"), new TestAction(leaveGame, "Start Game")};
		listenerStatic = new List<TestAction>(){new TestAction(listenerBackToInit, "Leave game")};
		listenerPostJoin = new List<TestAction>(){new TestAction(leaveGame, "Leave game")};
	}

	protected override void Start()
	{
		if ( enterAsHost )
		{
			sceneActions = broadcasterStatic;

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
			sceneActions = listenerStatic;

			TestNetClient.serverFoundEvent -= onServerFound;
			TestNetClient.serverFoundEvent += onServerFound;

			discoveryListener.startClient();
		}

		base.Start();
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

			sceneActions = new List<TestAction>(count + listenerStatic.Count);
			sceneActions.AddRange(listenerStatic);
			
			foreach ( KeyValuePair<string, NetworkBroadcastResult> kvp in discoveryListener.broadcastsReceived )
			{
				sceneActions.Add(new TestAction(()=>{joinGame(kvp.Value.serverAddress);}, kvp.Value.serverAddress));
			}
			
			setupActionButtons();
		}
		else
		{
			sceneActions = listenerStatic;
			setupActionButtons();
		}
	}

	private void joinGame(string _serverAddress)
	{
		sceneActions = listenerPostJoin;
		setupActionButtons();

		SceneActionsOnline.isLocalPlayerHost = false;

		Destroy(discoveryListener.gameObject);

		string portDataString = BytesToString(discoveryListener.broadcastsReceived[_serverAddress].broadcastData);
		gameServerNetworkPort = System.Convert.ToInt32(portDataString) + 1;

		Debug.Log("Joining as client to " + _serverAddress + " on port " + gameServerNetworkPort);

		endListening();

		NetworkManagerDiscovery.singleton.networkAddress = _serverAddress;
		NetworkManagerDiscovery.singleton.networkPort = gameServerNetworkPort;
		/*NetworkClient remoteClient = */NetworkManagerDiscovery.singleton.StartClient();
	}

	private void listenerBackToInit()
	{
		sceneActions = noActions;
		setupActionButtons();

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
