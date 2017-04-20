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

        if ( positionTappedEvent != null )
        {
            positionTappedEvent(position);
        }
    }
}
