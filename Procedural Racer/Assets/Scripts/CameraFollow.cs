using UnityEngine;
using System.Collections;

//Follows a set transform target with optional smoothing. Reused and refactored from Flyswatter3000.
public class CameraFollow : MonoBehaviour
{
    public Transform target; //Target to follow. Set this to car body.
    public float smoothTime = 0.0f; //Optional smoothing / delay.

	private Vector3 velocity = Vector3.zero; // Velocity reference for SmoothDamp

    void FixedUpdate()
    {
        Vector3 goalPos = target.position;
        goalPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
}
