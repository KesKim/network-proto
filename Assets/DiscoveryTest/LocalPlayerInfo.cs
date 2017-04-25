using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerInfo : MonoBehaviour
{
	public static NetworkConnection connection;
	public static PlayerController controller;

	public static void Initialize()
	{
		connection = null;
		controller = null;
	}
}
