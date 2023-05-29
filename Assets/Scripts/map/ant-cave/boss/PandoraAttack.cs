using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraAttack : ObjectPool
{
    public static PandoraAttack instance { get; private set; }

    [Header("Attack")]
    [SerializeField]
    public GameObject[] prefabBullet;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        prefabBullet = Resources.LoadAll<GameObject>("Prefabs/map/ant-cave/bullet");
        spawnPoint = transform.Find("AttackPoint").transform;
        poolManager = GameObject.Find("BulletPool").transform;
        LoadPandoraAttack();
    }

    public void LoadPandoraAttack()
    {
        LoadPrefabDB(prefabBullet);
        LoadBulletToPool(prefabBullet.Length, spawnPoint);
    }

    public void IceArrow()
    {
        GameObject bullet = GetBulletFromPool("IceArrow(Clone)");
        if (bullet != null)
        {
            bullet.transform.GetComponent<IceArrow>().moveSpeed = 5f;
            //attackPoint = GameObject.Find("Pandora").transform.GetChild(0).transform;
            //target = GameObject.Find("Player").transform.position;
            bullet.transform.position = spawnPoint.position;
            bullet.SetActive(true);
        }
    }

    public void NormalArrow()
    {
        GameObject bullet = GetBulletFromPool("FireArrow(Clone)");
        if (bullet != null)
        {
            bullet.transform.position = spawnPoint.position;
            bullet.SetActive(true);
        }
    }

    public void LightningArrow()
    {
        GameObject bullet = GetBulletFromPool("LightningBullet(Clone)");
        if (bullet != null)
        {
            bullet.SetActive(true);
        }
    }
}
