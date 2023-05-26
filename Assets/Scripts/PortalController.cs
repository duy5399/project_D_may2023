using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Portal Out").transform;
    }

    public void OnTriggerEnter2D(Collider other)
    {
        if(Vector2.Distance(transform.position, other.transform.position) > distance)
    }
}
