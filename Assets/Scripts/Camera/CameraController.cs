using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float smoothSpeed = 0.125f;
    [SerializeField] public Vector3 offset;
    [SerializeField] public Vector3 velocity;

    // LateUpdate is called after function Update
    void LateUpdate()
    {
        if(target.position.x > -7.5f && target.position.x < 7.5f)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(desiredPosition, offset, smoothSpeed * Time.deltaTime);
            this.transform.position = new Vector3(smoothedPosition.x, 0, smoothedPosition.z);
        }
        
    }
}
