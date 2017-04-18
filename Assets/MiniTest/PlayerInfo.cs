using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
    public NetworkIdentity networkIdentity;

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

	public int connectionId
	{
		get
		{
			if ( networkIdentity.connectionToClient != null )
			{
				return networkIdentity.connectionToClient.connectionId;
			}
			else
			{
				return networkIdentity.connectionToServer.connectionId;
			}
		}
	}

	public int uniquePlayerId;
}
