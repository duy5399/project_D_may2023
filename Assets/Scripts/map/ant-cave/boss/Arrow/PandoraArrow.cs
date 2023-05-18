using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraArrow : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform attackPoint;
    public Transform target;

    public float moveSpeed = 5f;

    public float attackPointX;
    public float targetX;

    public float distance;

    public float nextX;
    public float baseY;
    public float height;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        attackPointX = attackPoint.transform.position.x;
        targetX = target.transform.position.x;

        distance = targetX - attackPointX;

        nextX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        baseY = Mathf.Lerp(attackPoint.transform.position.y, target.transform.position.y, (nextX - attackPointX) / distance);
        height = 5 * (nextX - attackPointX) * (nextX - targetX) / (-0.25f * distance * distance);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }


    //public void OnCollisionEnter2D(Collision collision)
    //{
    //    Debug.Log("Hit player");
    //}
}
