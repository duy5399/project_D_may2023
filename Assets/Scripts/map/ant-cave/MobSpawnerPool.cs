using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerPool : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> prefabMob;
    public List<GameObject> poolMob;
    public Transform mobSpawnPoint;
    public float spawnInterval;
    public Transform poolManager;

    public void LoadPrefabMob(GameObject mob)
    {
        foreach (Transform prefabObj in mob.transform)
        {
            prefabMob.Add(prefabObj.gameObject);
        }
    }

    public void LoadMobToPool(int maxPool)
    {
        for (int i = 0; i < maxPool; i++)
        {
            int rand = Random.Range(0, prefabMob.Count);          
            GameObject mob = Instantiate(prefabMob[rand], mobSpawnPoint.position, Quaternion.identity);
            mob.transform.parent = poolManager;
            mob.SetActive(false);
            poolMob.Add(mob);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for(int i = 0; i < poolMob.Count; i++)
        {
            if (!poolMob[i].activeInHierarchy)
            {
                return poolMob[i];
            }
        }
        return null;
    }

    public void SpawnMob(int maxMob)
    {
        StartCoroutine(SpawnInterval(spawnInterval, maxMob));
    }

    public IEnumerator SpawnInterval(float spawnInterval, int maxMob)
    {
        for (int i = 0; i < maxMob; i++)
        {
            GameObject mob = GetObjectFromPool();
            if( mob != null )
            {
                mob.GetComponent<MobController>().currentHealth = mob.GetComponent<MobController>().maxHealth;
                mob.transform.position = mobSpawnPoint.position;
                mob.SetActive(true);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}



