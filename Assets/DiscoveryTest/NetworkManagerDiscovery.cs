using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerDiscovery : NetworkManager
{
    [SyncEvent]
    public static event System.Action playersChangedEvent;

    public static Dictionary<int, PlayerInfo> playersByControllerId;

	public List<int> freePlayerIds;

	public void resetPlayerIds()
	{
		freePlayerIds = new List<int>(){ 1, 2, 3 };
		playersByControllerId = new Dictionary<int, PlayerInfo>(3);

		Debug.Log("Available Player IDs reset, " + freePlayerIds.Count + " remaining.");
	}

	public override void OnClientConnect(NetworkConnection _connection)
	{
		LocalPlayerInfo.connection = _connection;

		#region Base implementation
		bool m_AutoCreatePlayer = true;

		if (!clientLoadedScene)
		{
			// Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
			ClientScene.Ready(_connection);

			if ( m_AutoCreatePlayer )
			{
				ClientScene.AddPlayer(0);
			}
		}
		#endregion

		Debug.Log("Connection PlayerController: " + ((_connection.playerControllers == null || _connection.playerControllers.Count < 1) ? "NULL or empty" : _connection.playerControllers.ToString()));
	}

	public override void OnClientDisconnect(NetworkConnection _connection)
	{
		#region Base implementation
		StopClient();
		#endregion
	}

	public override void OnServerRemovePlayer(NetworkConnection _connection, PlayerController _player)
	{
		PlayerInfo info = _player.gameObject.GetComponent<PlayerInfo>();

		#region Base implementation
		if (_player.gameObject != null)
		{
			NetworkServer.Destroy(_player.gameObject);
		}
		#endregion
	}

    public override void OnServerAddPlayer(NetworkConnection _connection, short _playerControllerId)
    {
		Debug.Log("Player joining: " + _connection.address + " " + _connection.connectionId + " " + _playerControllerId);

        GameObject playerObject = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        PlayerInfo info = playerObject.GetComponent<PlayerInfo>();

		info.networkIdentity = info.GetComponent<NetworkIdentity>();

		Debug.Log(freePlayerIds.Count);

		info.uniquePlayerId = freePlayerIds[0];
		freePlayerIds.RemoveAt(0);

		Debug.Log("Assigned ID " + info.uniquePlayerId + " - " + freePlayerIds.Count + " IDs remaining.");

		playerObject.name = "PlayerObject" + info.uniquePlayerId;

		if ( playersByControllerId.ContainsKey(_connection.connectionId) == false )
        {
			Debug.Log(string.Format("Added player {0} to list as connection {1}", info.uniquePlayerId, _connection.connectionId));
			playersByControllerId.Add(_connection.connectionId, info);
        }

        NetworkServer.AddPlayerForConnection(_connection, playerObject, _playerControllerId);

        if ( playersChangedEvent != null )
        {
            playersChangedEvent();
        }
    }

	public override void OnStopClient()
	{
		Debug.Log("OnStopClient");

        if ( NetworkManager.singleton != null && NetworkManager.singleton.client != null )
        {
		    base.OnStopClient();
        }
	}

	public override void OnServerDisconnect(NetworkConnection _connection)
	{
		int uniquePlayerId = playersByControllerId[_connection.connectionId].uniquePlayerId;

		Debug.LogWarning("Disconnecting player: " + uniquePlayerId);

		if ( freePlayerIds.Contains(uniquePlayerId) == false )
		{
			freePlayerIds.Add(uniquePlayerId);
			freePlayerIds.Sort();
		}

		playersByControllerId.Remove(_connection.connectionId);

		if ( _connection.lastError != NetworkError.Ok )
		{
			switch ( _connection.lastError )
			{
				case NetworkError.Ok:
				{
					break;
				}
				case NetworkError.Timeout:
				{
					Debug.LogWarning("Unity ServerDisconnected due to error: " + _connection.lastError);
					break;
				}
				default:
				{
					Debug.LogError("Unity ServerDisconnected due to error: " + _connection.lastError);
					break;	
				}
			}
		}

		NetworkServer.DestroyPlayersForConnection(_connection);
		// default implementation basically just calls the above line and prints errors if any
		//base.OnServerDisconnect(_connection);

		if ( playersChangedEvent != null )
		{
			playersChangedEvent();
		}
	}
}
