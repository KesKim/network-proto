using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DynamicPlayerView : MonoBehaviour
{
	[SerializeField] private GameObject prefab;

	[SerializeField] private RectTransform holderParent;

	private void Awake()
	{
		NetworkManagerDiscovery.playersChangedEvent -= updatePlayers;
		NetworkManagerDiscovery.playersChangedEvent += updatePlayers;
	}

	private void OnDestroy()
	{
		NetworkManagerDiscovery.playersChangedEvent -= updatePlayers;
	}

    public void updatePlayers()
    {
		Debug.Log("Player list changed. Updating player list UI.");
        clearPlayerTexts();

		int count = PlayerInfo.allPlayers.Count;

        for ( int i = 0; i < count; i++ )
        {
			createButton(PlayerInfo.allPlayers[i]);
        }
    }

    private void clearPlayerTexts()
    {
        int count = holderParent.childCount;

        for ( int i = count - 1; i > -1; i-- )
        {
            Destroy(holderParent.GetChild(i).gameObject);
        }
    }

	private void createButton(PlayerInfo _player)
    {
		GameObject item = (GameObject)Instantiate(prefab, holderParent);

        Text text = item.GetComponentInChildren<Text>();
		text.text = "P" + _player.uniquePlayerId + " - " + _player.playerName + " - " + _player.ipAddress;
		text.color = _player.playerColor;
    }
}
