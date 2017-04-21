using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private Renderer myRenderer;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        myRenderer.material.color = Color.green;

        InteractionPlane.positionTappedEvent -= moveToPosition;
        InteractionPlane.positionTappedEvent += moveToPosition;
    }

	private void OnDestroy()
	{
		InteractionPlane.positionTappedEvent -= moveToPosition;
	}

    private void moveToPosition(Vector3 _position)
    {
		if ( InteractionPlane.isAvailable == false )
		{
			InteractionPlane.positionTappedEvent -= moveToPosition;
			return;
		}

        if ( this.isLocalPlayer == false )
        {
            return;
        }

        this.transform.position = _position;
    }
}
