using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraSummon : ObjectPool
{
    public static PandoraSummon instance { get; private set; }

    [Header("SummonMob")]
    [SerializeField]
    private GameObject[] prefabMob;
    private int numberOfMob;


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
        prefabMob = Resources.LoadAll<GameObject>("Prefabs/map/ant-cave/mob");
        spawnPoint = GameObject.Find("MobSpawnPoint").transform;
        poolManager = GameObject.Find("MobPool").transform;
        LoadPandoraSummon();
    }

    public void LoadPandoraSummon()
    {
        numberOfMob = 3;
        spawnInterval = 0.75f;
        LoadPrefabDB(prefabMob);
        LoadMobToPool(numberOfMob, spawnPoint);
    }

    public void Summon()
    {
        SpawnMob(numberOfMob);
    }
}
