using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        attackInterval = 2f;
        numberOfAttack = 10;
        StartCoroutine(MoveToTargetWithMark(attackInterval, numberOfAttack));
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
        }
        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit ground");
        }
        //GameObject frost_effect = Instantiate(blastOut, transform.position, Quaternion.identity);
        //Destroy(gameObject);
    }
}
