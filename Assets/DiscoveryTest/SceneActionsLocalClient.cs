using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsLocalClient : SceneActions
{
    //public class ServerInfo
    //{
    //    string address;
    //    string data;

    //    public ServerInfo(string _address, string _data)
    //    {
    //        address = _address;
    //        data = _data;
    //    }
    //}

    public static string lastSelectedAddress;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
			new TestAction(()=>{joinGame("localhost");}, "localhost:10000")
        };
    }

    private void goBackToInit()
    {
        SceneManager.LoadScene("SceneInit");
    }

    private void joinGame(string _serverAddress)
    {
        SceneActionsOnline.isLocalPlayerHost = false;

        lastSelectedAddress = _serverAddress;

		int port = 10000;

		Debug.Log("Joining as client to " + _serverAddress + " on port " + port);

		if ( NetworkManagerDiscovery.singleton != null && NetworkManagerDiscovery.singleton.client == null )
        {
			SceneActionsOnline.serverNetworkAddress = _serverAddress;
			SceneActionsOnline.serverNetworkPort = port;
        }

        SceneManager.LoadScene("SceneOnline");
    }
}
