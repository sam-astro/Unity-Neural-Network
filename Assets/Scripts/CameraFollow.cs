/*
Copyright (c) Sam Hastings
*/

using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
	#endregion

	private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
