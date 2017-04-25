using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestNetServer : NetworkDiscovery
{
	public static int serverPortUsed;

    [SerializeField] private int minPort = 10000;
    [SerializeField] private int maxPort = 10010;
    [SerializeField] private int defaultPort = 10000;

    //Call to create a server
    public void startServer()
    {
        serverPortUsed = createServer();

        if ( serverPortUsed != -1 )
        {
            Debug.Log("Server created on port : " + serverPortUsed);
            broadcastData = serverPortUsed.ToString();
            Initialize();
            StartAsServer();
        }
        else
        {
            Debug.Log("Failed to create Server");
        }
    }

    //Creates a server then returns the port the server is created with. Returns -1 if server is not created
    private int createServer()
    {
		int attemptServerPort = -1;

        //Connect to default port
        bool serverCreated = NetworkServer.Listen(defaultPort);

        if ( serverCreated )
        {
            attemptServerPort = defaultPort;
            Debug.Log("Server Created with default port");
        }
        else
        {
            Debug.Log("Failed to create with the default port");

            //Try to create server with other port from min to max except the default port which we trid already
            for ( int tempPort = minPort; tempPort <= maxPort; tempPort++ )
            {
                //Skip the default port since we have already tried it
                if ( tempPort != defaultPort )
                {
                    //Exit loop if successfully create a server
                    if ( NetworkServer.Listen(tempPort) )
                    {
                        attemptServerPort = tempPort;
                        break;
                    }

                    //If this is the max port and server is not still created, show, failed to create server error
                    if ( tempPort == maxPort )
                    {
                        Debug.LogError("Failed to create server");
                    }
                }
            }
        }

        return attemptServerPort;
    }
}
