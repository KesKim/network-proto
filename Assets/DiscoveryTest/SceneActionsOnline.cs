using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneActionsOnline : SceneActions
{
    public static bool isLocalPlayerHost;

    [SerializeField] private GameObject interactionPlane;

    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
            new TestAction(leaveGame, "Leave game")
            , new TestAction(spawnStuff, "Spawn stuff")
        };

        if ( isLocalPlayerHost )
        {
            Debug.Log("Starting host");
            NetworkClient hostClient = CustomNetworkManager.singleton.StartHost();
        }
        else
        {
            Debug.Log("Starting client");
            NetworkClient remoteClient = CustomNetworkManager.singleton.StartClient();
        }
    }

    private void spawnStuff()
    {
        Debug.Log("Spawning");

        //NetworkServer.Spawn(interactionPlane);
        NetworkServer.SpawnObjects();
    }

    private void leaveGame()
    {
        if ( Network.isServer )
        {
            Debug.Log("Stopping host");
            CustomNetworkManager.singleton.StopHost();
        }
        else
        {
            Debug.Log("Stopping client");
            CustomNetworkManager.singleton.StopClient();
        }

        SceneManager.LoadScene("SceneInit");
    }
}
