using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
	[SyncVar(hook="onPlayerIdChanged")]
	public int uniquePlayerId = -1;

	[SyncVar(hook="onPlayerIpChanged")]
	public string ipAddress = null;

	[SyncVar(hook="onPlayerNameChanged")]
	public string playerName = null;

	[SyncVar(hook="onPlayerColorChanged")]
	public Color playerColor = Color.grey;

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

	private void onPlayerIdChanged(int _newId)
	{
		uniquePlayerId = _newId;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	private void onPlayerIpChanged(string _newIp)
	{
		ipAddress = _newIp;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	private void onPlayerNameChanged(string _playerName)
	{
		playerName = _playerName;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	[Command]
	private void CmdSetPlayerName(string _playerName)
	{
		playerName = _playerName;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	private void onPlayerColorChanged(Color _playerColor)
	{
		playerColor = _playerColor;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	[Command]
	private void CmdSetPlayerColor(Color _playerColor)
	{
		playerColor = _playerColor;
		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	private void OnDestroy()
	{
		if ( allPlayers != null )
		{
			Debug.Log("Removing player " + this.uniquePlayerId + ". allPlayers.Count " + allPlayers.Count);
			bool isRemoved = allPlayers.Remove(this);
			Debug.Log("Removal " + isRemoved + ". allPlayers.Count " + allPlayers.Count);
		}

		NetworkManagerDiscovery.triggerPlayersChangedEvent();
	}

	public override void OnStartServer()
	{
		ipAddress = networkIdentity.connectionToClient.address;
	}

	public override void OnStartClient()
	{
		Debug.Log("X player uniquePlayerId " + uniquePlayerId);

		if ( allPlayers.Contains(this) == false )
		{
			allPlayers.Add(this);
		}

		NetworkManagerDiscovery.triggerPlayersChangedEvent();

		CmdSetPlayerName(LocalPlayerInfo.playerName);
		CmdSetPlayerColor(LocalPlayerInfo.playerColor);
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
