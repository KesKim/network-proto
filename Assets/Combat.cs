using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour
{
	public bool destroyOnDeath;
	public const int maxHealth = 100;

	[SyncVar]
	public int health = maxHealth;

	public void TakeDamage(int amount)
	{
		if ( isServer == false )
		{
			return;
		}

		health -= amount;

		if ( health <= 0 )
		{
			if ( destroyOnDeath )
			{
				Destroy(gameObject);
			}
			else
			{
				health = maxHealth;
				
				RpcRespawn();
			}
		}
	}

	[ClientRpc]
	private void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// move back to zero location
			transform.position = Vector3.zero;
		}
	}
}
