using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MiniConnectDebug : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    [SyncVar] private string textData;

    private void Awake()
    {
        CustomNetworkManager.playerJoinedEvent -= updateText;
        CustomNetworkManager.playerJoinedEvent += updateText;

        updateText();
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            updateText();
        }
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);

        Debug.Log("OnDeserialize");

        updateText();
    }

    private void updateText()
    {
        Debug.Log("updateText");

        if ( this.isServer )
        {
            Debug.Log("Building text");

            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            int count = CustomNetworkManager.playersByControllerId == null ? 0 : CustomNetworkManager.playersByControllerId.Count;
            Debug.Log(count);

            builder.Append("(S) ");
            builder.Append(NetworkServer.serverHostId);
            builder.Append(" ");
            builder.AppendLine(this.playerControllerId.ToString());

            foreach ( KeyValuePair<short, PlayerInfo> kvp in CustomNetworkManager.playersByControllerId )
            {
                builder.Append("(C) ");
                builder.Append(kvp.Value.playerControllerId);
                builder.Append(" ");
                builder.AppendLine(kvp.Value.connectionToServer.address);
                //builder.Append(NetworkClient.allClients[i].connection.address);
                //builder.Append(" ");
                //builder.AppendLine(NetworkClient.allClients[i].connection.connectionId.ToString());
            }

            textData = builder.ToString();
        }

        text.text = textData;
    }
}
