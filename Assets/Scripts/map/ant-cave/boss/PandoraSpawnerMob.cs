using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraSpawnerMob : MobSpawnerPool
{
    public static PandoraSpawnerMob instance;

    [Header("SummonMob")]
    public int amountMob;
    public GameObject mobs;

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
        mobs = GameObject.Find("Mobs");
        mobSpawnPoint = GameObject.Find("MobSpawnPoint").transform;
        poolManager = GameObject.Find("PoolManager").transform;
        amountMob = 3;
        spawnInterval = 0.5f;
        LoadPrefabMob(mobs);
        LoadMobToPool(amountMob);
    }

    public void SummonMobs()
    {
        SpawnMob(amountMob);
    }
}
