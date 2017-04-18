using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    [SyncEvent]
    public static event System.Action playersChangedEvent;

    public static Dictionary<int, PlayerInfo> playersByControllerId;

	public List<int> freePlayerIds;

	private void Awake()
	{
		freePlayerIds = new List<int>(){1, 2};
	}

    public override void OnServerAddPlayer(NetworkConnection _connection, short _playerControllerId)
    {
		Debug.Log("Player joining: " + _connection.address + " " + _connection.connectionId + " " + _playerControllerId);

        GameObject playerObject = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.name = "PlayerObject" + _playerControllerId;

        PlayerInfo info = playerObject.GetComponent<PlayerInfo>();

		info.networkIdentity = info.GetComponent<NetworkIdentity>();

        if ( playersByControllerId == null )
        {
            playersByControllerId = new Dictionary<int, PlayerInfo>();
        }

		info.uniquePlayerId = freePlayerIds[0];
		freePlayerIds.RemoveAt(0);

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

	public override void OnServerRemovePlayer(NetworkConnection _connection, PlayerController _player)
	{
		int uniquePlayerId = _player.gameObject.GetComponent<PlayerInfo>().uniquePlayerId;

		if ( freePlayerIds.Contains(uniquePlayerId) == false )
		{
			freePlayerIds.Add(uniquePlayerId);
			freePlayerIds.Sort();
		}

		if ( playersByControllerId.Remove(_connection.connectionId) )
		{
			if ( playersChangedEvent != null )
			{
				playersChangedEvent();
			}
		}

		base.OnServerRemovePlayer(_connection, _player);
	}
}
