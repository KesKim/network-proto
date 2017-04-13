using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
	[SerializeField] private GameObject bulletPrefab;

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		this.GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	private void Update()
	{
		if ( isLocalPlayer == false )
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * 0.1f;
		var z = Input.GetAxis("Vertical") * 0.1f;

		var absX = Mathf.Abs(x);
		var absZ = Mathf.Abs(z);

		float deadzone = 0.005f;

		//if ( absX > deadzone && absZ > deadzone )
		{
			if ( absX > absZ )
			{
				if ( x > 0 )
				{
					transform.rotation = Quaternion.LookRotation(Vector3.right);
				}
				else
				{
					transform.rotation = Quaternion.LookRotation(Vector3.left);
				}
			}
			else
			{
				if ( z > 0 )
				{
					transform.rotation = Quaternion.LookRotation(Vector3.forward);
				}
				else
				{
					transform.rotation = Quaternion.LookRotation(Vector3.back);
				}
			}
		}

		transform.Translate(new Vector3(x, 0f, z), Space.World);

		if ( Input.GetKeyDown(KeyCode.Space) )
		{
			CmdFire();
		}
	}

	[Command]
	private void CmdFire()
	{
		// create the bullet object from the bullet prefab
		var bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward * 1.3f, Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = transform.forward * 4f;

		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
