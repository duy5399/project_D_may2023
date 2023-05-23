using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletWithMark : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        attackPoint = GameObject.Find("Pandora").transform.GetChild(1).transform;
        target = GameObject.Find("Player").transform.position;
        attackInterval = 2f;
        numberOfAttack = 5;
        StartCoroutine(MoveToTargetWithMark(attackInterval, numberOfAttack));
    }
}
