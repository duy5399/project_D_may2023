using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraArrow : MonoBehaviour
{
    public Rigidbody2D rd;
    float moveSpeed = 2f;
    float angle;

    void Start()
    {
        rd= GetComponent<Rigidbody2D>();
        rd.velocity = Vector2.left * moveSpeed;
    }

    void FixedUpdate()
    {
        
        Vector2 v = rd.velocity;
        angle = Mathf.Atan2(v.y, v.x) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3.Slerp
    }

    //public void OnCollisionEnter2D(Collision collision)
    //{
    //    Debug.Log("Hit player");
    //}
}
