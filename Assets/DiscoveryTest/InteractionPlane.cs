using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractionPlane : NetworkBehaviour
{
    public static event System.Action<Vector3> positionTappedEvent;

    private void OnMouseDown()
    {
        Debug.Log("Plane tapped");
        Vector3 position = Vector3.zero;

        if ( Input.touchCount > 0 )
        {
            position = Input.GetTouch(0).position;
        }
        else if ( Input.GetMouseButtonDown(0) )
        {
            position = Input.mousePosition;
        }

		Plane plane = new Plane(Vector3.up, Vector3.zero);

		Ray ray = Camera.main.ScreenPointToRay(position);
		float distance;

		if ( plane.Raycast(ray, out distance) )
		{
			position = ray.GetPoint(distance);
		}

        if ( positionTappedEvent != null )
        {
            positionTappedEvent(position);
        }
    }
}
