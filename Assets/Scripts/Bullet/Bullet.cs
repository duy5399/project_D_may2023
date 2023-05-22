﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform attackPoint;
    public Transform player;
    public Vector3 target;

    public float alertInterval;
    public float attackInterval;
    public int numberOfAttack;
    public GameObject prefabBullet;
    public GameObject blastOut;

    public float moveSpeed;

    public float attackPointX;
    public float targetX;

    public float distance;

    public float nextX;
    public float baseY;
    public float height;



    public void MoveToTarget()
    {
        attackPointX = attackPoint.transform.position.x;
        targetX = target.x;

        distance = targetX - attackPointX;

        nextX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        baseY = Mathf.Lerp(attackPoint.transform.position.y, -2.246191f, (nextX - attackPointX) / distance); //target.y = -2.246191f
        height = 3 * (nextX - attackPointX) * (nextX - targetX) / (-0.25f * distance * distance);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    //spawn bullet on position of player
    public IEnumerator MoveToTargetWithMark(float alertInterval, float attackInterval, int numberOfAttack, GameObject prefabBullet)
    {
        GameObject bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        bullet.SetActive(false);
        for (int i = 0; i < numberOfAttack; i++)
        {
            Vector3 attackPoint = GameObject.Find("Player").transform.position;
            transform.position = new Vector3(attackPoint.x, -2.246191f, 0);
            yield return new WaitForSeconds(alertInterval);
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            yield return new WaitForSeconds(attackInterval);           
            bullet.SetActive(false);
        }
        Destroy(bullet);
        Destroy(gameObject);
    }

}