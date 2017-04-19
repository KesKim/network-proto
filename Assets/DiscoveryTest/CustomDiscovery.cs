using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomDiscovery : NetworkDiscovery
{
    public void startToBroadcastAsHost()
    {
    }

    public void startToListenForBroadcasts()
    {
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
    }
}
