using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapData", menuName = "ScriptableObjects/Map Data")]
public class MapDataSO : ScriptableObject
{
    [SerializeField] private List<ModeMap> map;
    public List<ModeMap> map_ => map;
}

[Serializable]
public class ModeMap
{
    [SerializeField] private List<MapSO> mapDifficulty = new List<MapSO>();

    public List<MapSO> mapDifficulty_ => mapDifficulty;
}
