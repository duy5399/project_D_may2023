using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> prefabList;
    public List<GameObject> objPool;
    public Transform spawnPoint;
    public float spawnInterval;
    public Transform poolManager;

    public void LoadPrefabDB(GameObject[] prefabDB)
    {
        foreach (GameObject prefab in prefabDB)
        {
            prefabList.Add(prefab.gameObject);
        }
    }

    public void LoadMobToPool(int numberOfMob, Transform point)
    {
        for (int i = 0; i < numberOfMob; i++)
        {
            int rand = Random.Range(0, prefabList.Count);          
            GameObject mob = Instantiate(prefabList[rand], point.position, Quaternion.identity);
            mob.transform.parent = poolManager;
            mob.SetActive(false);
            objPool.Add(mob);
        }
    }

    public GameObject GetMobFromPool()
    {
        for(int i = 0; i < objPool.Count; i++)
        {
            if (!objPool[i].activeInHierarchy)
            {
                return objPool[i];
            }
        }
        LoadMobToPool(1, spawnPoint);
        return objPool[objPool.Count - 1];
    }

    public void SpawnMob(int numberOfMob)
    {
        StartCoroutine(SpawnInterval(spawnInterval, numberOfMob));
    }

    public IEnumerator SpawnInterval(float spawnInterval, int numberOfMob)
    {
        for (int i = 0; i < numberOfMob; i++)
        {
            GameObject mob = GetMobFromPool();
            if( mob != null )
            {
                //mob.GetComponent<MobController>().currentHealth = mob.GetComponent<MobController>().maxHealth;
                mob.GetComponent<MobController>().LoadMobController();
                mob.transform.position = spawnPoint.position;
                mob.SetActive(true);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void LoadBulletToPool(int numberOfMob, Transform point)
    {
        for (int i = 0; i < numberOfMob; i++)
        {
            GameObject mob = Instantiate(prefabList[i], point.position, Quaternion.identity);
            mob.transform.parent = poolManager;
            mob.SetActive(false);
            objPool.Add(mob);
        }
    }

    public GameObject GetBulletFromPool(string nameBullet)
    {
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].name == nameBullet && !objPool[i].activeInHierarchy)
            {
                return objPool[i];
            }
        }
        return null;
    }
}



