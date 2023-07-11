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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamage(damage/2, 0.5f);
            collider.gameObject.GetComponent<PlayerCombat>().SlowEffect(3f);
            Debug.Log("Hit player");
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Hit ground");
        }
        GameObject blastout = Instantiate(blastOut, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
