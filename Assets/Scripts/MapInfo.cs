using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public static MapInfo instance { get; private set; }

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField] private MapSO mapInfo;
    public MapSO mapInfo_ => mapInfo;

    public void SetMapInfo(MapSO _mapInfo)
    {
        this.mapInfo = _mapInfo;
    }

    public MapSO GetMapInfo()
    {
        return mapInfo;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
