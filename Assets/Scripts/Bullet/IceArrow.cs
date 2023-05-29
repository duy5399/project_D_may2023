using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : Bullet
{
    void FixedUpdate()
    {
        MoveToTarget();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerMovement>().DazedTime();
            Debug.Log("Hit player");
        }
        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit ground");
        }
        GameObject blastout = Instantiate(blastOut, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
