using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MiniConnectDebug : NetworkBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

	[SyncVar(hook="updateText")] private string textData;

	public override void OnStartServer()
	{
		base.OnStartServer();

		CustomNetworkManager.playersChangedEvent -= refreshText;
		CustomNetworkManager.playersChangedEvent += refreshText;

		refreshText();
	}

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            updateText();
        }
    }

//    public override void OnDeserialize(NetworkReader reader, bool initialState)
//    {
//        base.OnDeserialize(reader, initialState);
//
//        Debug.Log("OnDeserialize");
//
//        updateText();
//    }

	private void refreshText()
	{
		if ( this.isServer )
		{
			buildText();
		}

		updateText();
	}

	// Server only
	private void buildText()
	{
		Debug.Log("Building text");
		
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		
		builder.Append("(S) ");
		builder.AppendLine(NetworkServer.serverHostId.ToString());

		if ( CustomNetworkManager.playersByControllerId != null )
		{
			foreach ( KeyValuePair<int, PlayerInfo> kvp in CustomNetworkManager.playersByControllerId )
			{
				builder.Append("(C) ");
				builder.Append(kvp.Value.connectionId);
				builder.Append(" player ");
				builder.AppendLine(kvp.Value.uniquePlayerId.ToString());
			}
		}

		
		textData = builder.ToString();
	}

	private void updateText(string _text = null)
    {
		if ( (string.IsNullOrEmpty(_text) && this.isServer) == false )
		{
			textData = _text;
		}

		Debug.Log("updateText: " + _text);

        text.text = textData;
    }
}
