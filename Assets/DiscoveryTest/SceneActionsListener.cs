using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsListener : SceneActions
{
    public class ServerInfo
    {
        string address;
        string data;

        public ServerInfo(string _address, string _data)
        {
            address = _address;
            data = _data;
        }
    }

    public static string lastSelectedAddress;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(stopListening, "Stop listening")
        };

        TestNetClient.serverFoundEvent -= onServerFound;
        TestNetClient.serverFoundEvent += onServerFound;
    }

    private void stopListening()
    {
        TestNetClient listener = GameObject.FindObjectOfType<TestNetClient>();
        Destroy(listener.gameObject);

        SceneManager.LoadScene("SceneInit");
    }

    private void onServerFound(string _fromAddress, string _data)
    {
        if ( TestNetClient.Instance.broadcastsReceived != null && TestNetClient.Instance.broadcastsReceived.Count > 0 )
        {
            int count = TestNetClient.Instance.broadcastsReceived.Count;
            sceneActions = new List<TestAction>(count + 1);
            sceneActions.Add(new TestAction(stopListening, "Stop listening"));

            foreach ( KeyValuePair<string, NetworkBroadcastResult> kvp in TestNetClient.Instance.broadcastsReceived )
            {
                sceneActions.Add(new TestAction(()=>{joinGame(kvp.Value.serverAddress);}, kvp.Key));
            }
        }
        else
        {
            sceneActions = new List<TestAction>()
            {
                new TestAction(stopListening, "Stop listening")
            };
        }
    }

    private void joinGame(string _serverAddress)
    {
        lastSelectedAddress = _serverAddress;

        TestNetClient listener = GameObject.FindObjectOfType<TestNetClient>();
        Destroy(listener.gameObject);

        SceneManager.LoadScene("SceneOnline");
    }
}
