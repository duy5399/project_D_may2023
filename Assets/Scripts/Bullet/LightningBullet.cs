using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : Bullet
{
    [SerializeField] private float attackInterval;
    [SerializeField] private int numberOfAttack;

    // Start is called before the first frame update
    void Start()
    {
        attackInterval = 2f;
        numberOfAttack = 2;
        StartCoroutine(MoveToTargetWithMark(attackInterval, numberOfAttack));
    }

    //spawn bullet on position of player
    public IEnumerator MoveToTargetWithMark(float _attackInterval, int _numberOfAttack)
    {
        for (int i = 0; i < _numberOfAttack; i++)
        {
            Vector3 targetPoint = GameObject.Find("Player").transform.position;
            transform.position = new Vector3(targetPoint.x, -2.246191f, 0);
            yield return new WaitForSeconds(_attackInterval);
        }
        gameObject.SetActive(false);
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
