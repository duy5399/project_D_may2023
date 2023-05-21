using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalArrow : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        attackPoint = GameObject.Find("Pandora").transform.GetChild(1).transform;
        target = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToTarget();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamage(10);
            Debug.Log("Hit player");
        }
        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit ground");           
        }
        GameObject blastout = Instantiate(blastOut, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
