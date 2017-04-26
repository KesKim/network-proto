using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLocalPlayerName : MonoBehaviour
{
	[SerializeField] private InputField inputField;

	public void OnChangedValue()
	{
		LocalPlayerInfo.playerName = inputField.text;
	}
}
