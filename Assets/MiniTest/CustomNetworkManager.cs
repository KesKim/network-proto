using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    [SyncEvent]
    public static event System.Action playerJoinedEvent;

    public static Dictionary<short, PlayerInfo> playersByControllerId;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log(conn.address + " " + conn.connectionId);

        GameObject playerObject = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.name = "PlayerObject" + playerControllerId;

        PlayerInfo info = playerObject.GetComponent<PlayerInfo>();

        for ( int i = conn.playerControllers.Count - 1; i > -1; i-- )
        {
            if ( conn.playerControllers[i].playerControllerId == playerControllerId )
            {
                info.networkIdentity = conn.playerControllers[0].unetView;
            }
        }

        if ( playersByControllerId == null )
        {
            playersByControllerId = new Dictionary<short, PlayerInfo>();
        }

        if ( playersByControllerId.ContainsKey(playerControllerId) == false )
        {
            playersByControllerId.Add(playerControllerId, info);
        }

        NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);

        if ( playerJoinedEvent != null )
        {
            playerJoinedEvent();
        }
    }
}
