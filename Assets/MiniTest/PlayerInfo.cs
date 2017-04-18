using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar] public string text;

    public NetworkIdentity networkIdentity;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        text = Network.player.ipAddress;
    }
}
