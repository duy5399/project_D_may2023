using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraSummonMob : MobPool
{
    public static PandoraSummonMob instance;

    [Header("SummonMob")]
    public GameObject[] prefabDB;
    public int numberOfMob;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        prefabDB = Resources.LoadAll<GameObject>("Prefabs/map/ant-cave/mob");
        spawnPoint = GameObject.Find("MobSpawnPoint").transform;
        poolManager = GameObject.Find("PoolManager").transform;
        numberOfMob = 3;
        spawnInterval = 0.5f;
        LoadPrefabMob(prefabDB);
        LoadMobToPool(numberOfMob);
    }

    public void SummonMobs()
    {
        SpawnMob(numberOfMob);
    }
}
