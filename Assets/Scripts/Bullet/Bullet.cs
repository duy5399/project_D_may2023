using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected Transform attackPoint;
    [SerializeField]
    protected Vector3 target;
    [SerializeField]
    protected float attackInterval;
    [SerializeField]
    protected int numberOfAttack;

    [SerializeField]
    protected GameObject blastOut;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float attackPointX;
    [SerializeField]
    protected float targetX;
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected float nextX;
    [SerializeField]
    protected float baseY;
    [SerializeField]
    protected float height;

    public void SetParameter(float speed, Transform atkPoint, Vector3 tg)
    {
        moveSpeed = speed;
        attackPoint = atkPoint;
        target = tg;
    }

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
