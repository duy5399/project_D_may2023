using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "ScriptableObjects/Map")]
public class MapSO : ScriptableObject
{
    [SerializeField] private int mapIndex;
    [SerializeField] private string mapName;
    [SerializeField] private string mapDescription;
    [SerializeField] private Sprite mapImage;
    [SerializeField] private Object sceneToLoad;
    [SerializeField] private List<GameObject> mapMiniBoss;
    [SerializeField] private List<GameObject> mapBoss;
    [SerializeField] private List<MapRewards> mapRewards;
    [SerializeField] private int mapTime;

    public int mapIndex_ => mapIndex;
    public string mapName_ => mapName;
    public string mapDescription_ => mapDescription;
    public Sprite mapImage_ => mapImage;
    public Object sceneToLoad_ => sceneToLoad;
    public List<GameObject> mapMiniBoss_ => mapMiniBoss;
    public List<GameObject> mapBoss_ => mapBoss;
    public List<MapRewards> mapRewards_ => mapRewards;
    public int mapTime_ => mapTime;

}

[System.Serializable]
public class MapRewards{
    [SerializeField] private ItemSO item;
    [SerializeField] private int quantity;
    [SerializeField] private float dropRate;

    public ItemSO item_ => item;
    public int quantity_ => quantity;
    public float dropRate_ => dropRate;

    public MapRewards(ItemSO item, int quantity, float dropRate)
    {
        this.item = item;
        this.quantity = quantity;
        this.dropRate = dropRate;
    }
}
