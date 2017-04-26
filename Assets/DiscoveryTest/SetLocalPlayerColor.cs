using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocalPlayerColor : MonoBehaviour
{
	[SerializeField] private Color color;

	public void onSelect()
	{
		LocalPlayerInfo.playerColor = color;
	}
}
