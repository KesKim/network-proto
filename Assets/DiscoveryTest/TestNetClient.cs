using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestNetClient : NetworkDiscovery
{
    public static event System.Action<string, string> serverFoundEvent;

    public static TestNetClient Instance;

    private void Awake()
    {
        if ( Instance != null )
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        startClient();
    }

    public void startClient()
    {
        Initialize();
        StartAsClient();
    }

    public override void OnReceivedBroadcast(string _fromAddress, string _data)
    {
        Debug.Log("Server Found at " + _fromAddress + " with data: " + _data);

        if ( serverFoundEvent != null )
        {
            serverFoundEvent(_fromAddress, _data);
        }
    }

    private void OnDestroy()
    {
        if ( this == Instance )
        {
            Instance = null;
        }
    }
}
