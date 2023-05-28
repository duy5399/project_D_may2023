using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform attackPoint;
    protected Vector3 target;

    protected float attackInterval;
    protected int numberOfAttack;

    [SerializeField]
    protected GameObject blastOut;

    protected float moveSpeed;

    protected float attackPointX;
    protected float targetX;

    protected float distance;

    protected float nextX;
    protected float baseY;
    protected float height;



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
    public IEnumerator MoveToTargetWithMark(float attackInterval, int numberOfAttack)
    {
        for (int i = 0; i < numberOfAttack; i++)
        {
            Vector3 targetPoint = GameObject.Find("Player").transform.position;
            transform.position = new Vector3(targetPoint.x, -2.246191f, 0);
            yield return new WaitForSeconds(attackInterval);
        }
        gameObject.SetActive(false);
    }

}
