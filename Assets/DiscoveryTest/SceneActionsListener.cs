using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsListener : SceneActions
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
            new TestAction(stopListening, "Stop listening")
        };

        TestNetClient.serverFoundEvent -= onServerFound;
        TestNetClient.serverFoundEvent += onServerFound;
    }

    private void stopListening()
    {
        TestNetClient listener = GameObject.FindObjectOfType<TestNetClient>();
        listener.StopBroadcast();
        Destroy(listener.gameObject);

        SceneManager.LoadScene("SceneInit");
    }

    private void onServerFound(string _fromAddress, string _data)
    {
        Debug.Log(TestNetClient.Instance.broadcastsReceived == null ? "Null broadcast Dict." : TestNetClient.Instance.broadcastsReceived.Count < 1 ? "Empty broadcast Dict." : TestNetClient.Instance.broadcastsReceived.Count + " entries known.");

        if ( TestNetClient.Instance.broadcastsReceived != null && TestNetClient.Instance.broadcastsReceived.Count > 0 )
        {
            int count = TestNetClient.Instance.broadcastsReceived.Count;
            sceneActions = new List<TestAction>(count + 1);
            sceneActions.Add(new TestAction(stopListening, "Stop listening"));

            foreach ( KeyValuePair<string, NetworkBroadcastResult> kvp in TestNetClient.Instance.broadcastsReceived )
            {
                Debug.Log("Hi");

                sceneActions.Add(new TestAction(()=>{joinGame(kvp.Value.serverAddress);}, kvp.Key));
            }

            setupActionButtons();
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

        string portDataString = BytesToString(TestNetClient.Instance.broadcastsReceived[_serverAddress].broadcastData);

        if ( NetworkManager.singleton != null && NetworkManager.singleton.client == null )
        {
            NetworkManager.singleton.networkAddress = _serverAddress;
            NetworkManager.singleton.networkPort = System.Convert.ToInt32(portDataString);
            NetworkManager.singleton.StartClient();
        }

        SceneManager.LoadScene("SceneOnline");
    }

    static string BytesToString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
}
