using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
	public int uniquePlayerId;
    public NetworkIdentity networkIdentity;

	public override void OnStartLocalPlayer()
	{
		Debug.Log("Local player uniquePlayerId " + uniquePlayerId);
	}

	public bool isRemoteClient
	{
		get
		{
			return (networkIdentity.isServer == false);
		}
	}

	public bool isHostClient
	{
		get
		{
			return networkIdentity.isServer;
		}
	}

	public NetworkConnection connection
	{
		get
		{
			if ( networkIdentity.connectionToClient != null )
			{
				return networkIdentity.connectionToClient;
			}
			else
			{
				return networkIdentity.connectionToServer;
			}
		}
	}
}
