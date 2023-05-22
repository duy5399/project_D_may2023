using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPool : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> prefabList;
    public List<GameObject> mobPool;

    public Transform spawnPoint;
    public float spawnInterval;
    public Transform poolManager;

    public void LoadPrefabMob(GameObject[] prefabDB)
    {
        foreach (GameObject prefab in prefabDB)
        {
            prefabList.Add(prefab.gameObject);
        }
    }

    public void LoadMobToPool(int numberOfMob)
    {
        for (int i = 0; i < numberOfMob; i++)
        {
            int rand = Random.Range(0, prefabList.Count);          
            GameObject mob = Instantiate(prefabList[rand], spawnPoint.position, Quaternion.identity);
            mob.transform.parent = poolManager;
            mob.SetActive(false);
            mobPool.Add(mob);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for(int i = 0; i < mobPool.Count; i++)
        {
            if (!mobPool[i].activeInHierarchy)
            {
                return mobPool[i];
            }
        }
        return null;
    }

    public void SpawnMob(int numberOfMob)
    {
        StartCoroutine(SpawnInterval(spawnInterval, numberOfMob));
    }

    public IEnumerator SpawnInterval(float spawnInterval, int numberOfMob)
    {
        for (int i = 0; i < numberOfMob; i++)
        {
            GameObject mob = GetObjectFromPool();
            if( mob != null )
            {
                mob.GetComponent<MobController>().currentHealth = mob.GetComponent<MobController>().maxHealth;
                mob.transform.position = spawnPoint.position;
                mob.SetActive(true);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}



