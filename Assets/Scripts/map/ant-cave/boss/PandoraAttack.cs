using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PandoraAttack : ObjectPool
{
    public static PandoraAttack instance { get; private set; }

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
        poolManager = GameObject.Find("BulletPool").transform;
    }

    //tải object từ list data vào pool
    public void LoadBulletToAttack(List<GameObject> _prefabBullet)
    {
        LoadPrefabDB(_prefabBullet);
        LoadObjectToPool();
    }

    //tải object từ list data vào pool
    protected override void LoadObjectToPool()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            GameObject mob = Instantiate(prefabList[i]);
            mob.transform.parent = poolManager;
            mob.SetActive(false);
            objPool.Add(mob);
        }
    }

    //lấy object từ pool
    protected override GameObject GetObjectFromPool(GameObject _gameObject)
    {
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i] == _gameObject && !objPool[i].activeInHierarchy)
            {
                return objPool[i];
            }
        }
        GameObject mob = Instantiate(_gameObject);
        mob.transform.parent = poolManager;
        mob.SetActive(false);
        objPool.Add(mob);
        return objPool[objPool.Count - 1];
    }

    public void BeatD_IceArrow(float _speed, Transform _atkPoint, Vector3 _target, Transform _spawnPoint)
    {
        if(objPool.Find(x => x.name == "IceArrow(Clone)")){
            Debug.Log("Tìm thấy ice arrow: " + objPool.Find(x => x.name == "IceArrow(Clone)").name);
        }
        GameObject bullet = GetObjectFromPool(objPool.Find(x => x.name == "IceArrow(Clone)"));
        if (bullet != null)
        {
            bullet.transform.GetComponent<IceArrow>().SetParameter(_speed, _atkPoint, _target);
            bullet.transform.position = _spawnPoint.position;
            bullet.SetActive(true);
        }
    }

    public void BeatB_FireArrow(float _speed, Transform _atkPoint, Vector3 _target, Transform _spawnPoint)
    {
        GameObject bullet = GetObjectFromPool(objPool.Find(x => x.name == "FireArrow(Clone)"));
        if (bullet != null)
        {
            bullet.transform.GetComponent<FireArrow>().SetParameter(_speed, _atkPoint, _target);
            bullet.transform.position = _spawnPoint.position;
            bullet.SetActive(true);
        }
    }

    public void BeatC_LightningArrow()
    {
        GameObject bullet = GetObjectFromPool(objPool.Find(x => x.name == "LightningBullet(Clone)"));
        if (bullet != null)
        {
            bullet.SetActive(true);
        }
    }
}
