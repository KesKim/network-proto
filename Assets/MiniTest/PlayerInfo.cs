using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
	public int uniquePlayerId = -1;
    public NetworkIdentity networkIdentity;

	public static List<PlayerInfo> allPlayers;

	private void Awake()
	{
		if ( allPlayers == null )
		{
			allPlayers = new List<PlayerInfo>();
		}

		if ( networkIdentity == null )
		{
			networkIdentity = this.GetComponent<NetworkIdentity>();
		}
	}

	private void OnDestroy()
	{
		if ( allPlayers != null )
		{
			Debug.Log("Removing player " + this.uniquePlayerId + ". allPlayers.Count " + allPlayers.Count);
			bool isRemoved = allPlayers.Remove(this);
			Debug.Log("Removal " +isRemoved + ". allPlayers.Count " + allPlayers.Count);
		}

		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	public override void OnStartClient()
	{
		Debug.Log("X player uniquePlayerId " + uniquePlayerId);

		if ( allPlayers.Contains(this) == false )
		{
			allPlayers.Add(this);
		}

		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

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
