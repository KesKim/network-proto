using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedInputField : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_InputField input;
    [SerializeField] private Collider2D inputCollider;

    [SerializeField] private bool forHost;
    [SerializeField] private bool forRemoteClient;

    public override void OnStartLocalPlayer()
    {
        

        Debug.Log(Network.peerType);

        if ( (forHost && Network.isServer) || (forRemoteClient && Network.isClient) )
        {
            //inputCollider.enabled = true;
            input.readOnly = false;
        }
    }
}
