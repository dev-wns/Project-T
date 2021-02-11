using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 position = Vector3.zero;
    private Transform target;
    private Camera camera;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag( "Player" ).transform;
        camera = GetComponent<Camera>();
        position.z = camera.transform.position.z;
    }

    private void LateUpdate()
    {
        if ( ReferenceEquals( target, null ) )
        {
            return;
        }

        position.x = target.position.x;
        position.y = target.position.y;

        float orthoWidthOffset = ( Screen.width / 2.0f ) / ( Screen.height / 2.0f );
        if ( target.transform.position.x < -orthoWidthOffset * ( ( Screen.height / 2.0f ) - camera.orthographicSize ) ||
             target.transform.position.x >  orthoWidthOffset * ( ( Screen.height / 2.0f ) - camera.orthographicSize ) )
        {
            position.x = transform.position.x;
        }

        if ( target.transform.position.y < -( ( Screen.height / 2.0f ) - camera.orthographicSize ) ||
             target.transform.position.y >  ( ( Screen.height / 2.0f ) - camera.orthographicSize ) )
        {
            position.y = transform.position.y;
        }

        transform.position = position;
    }
}
