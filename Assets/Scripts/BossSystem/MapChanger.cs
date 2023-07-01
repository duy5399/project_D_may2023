using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    [SerializeField] private MapSO[] mapSO;
    [SerializeField] private int currentIndexMap;
    void Start()
    {
        ChangerMap(0);
    }

    public void ChangerMap(int _change)
    {
        currentIndexMap += _change;

        if (currentIndexMap < 0)
        {
            currentIndexMap = mapSO.Length - 1;
        }
        else if (currentIndexMap > mapSO.Length - 1)
        {
            currentIndexMap = 0;
        }
        MapInfo.instance.SetMapInfo(mapSO[currentIndexMap]);
        MapDisplay.instance.DisplayMap(mapSO[currentIndexMap]);
    }
}
