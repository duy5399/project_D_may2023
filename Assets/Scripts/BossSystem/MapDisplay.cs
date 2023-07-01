using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    public static MapDisplay instance { get; private set; }

    [SerializeField] private Image mapImage;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Image bossImage;
    [SerializeField] private List<Transform> mapReward;
    [SerializeField] private Button startBtn;

    [SerializeField] private UnityAction startAction;

    void Awake()
    {
        if(instance !=  null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        mapImage = transform.GetChild(1).GetComponent<Image>();
        bossName = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        bossImage = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        foreach(Transform t in transform.GetChild(2).GetChild(2))
        {
            mapReward.Add(t);
        }
        startBtn = transform.GetChild(3).GetComponent<Button>();
    }

    public void DisplayMap(MapSO _map)
    {
        mapImage.sprite = _map.mapImage_;
        bossName.text = _map.mapBoss_[_map.mapBoss_.Count - 1].name;
        bossImage.sprite = _map.mapBoss_[_map.mapBoss_.Count - 1].GetComponent<SpriteRenderer>().sprite;
        for(int i = 0; i < mapReward.Count; i++)
        {
            if (mapReward[i] != null)
            {
                mapReward[i].GetComponent<Image>().sprite = _map.mapRewards_[i].item_.itemIcon_;
            }
        }
        startBtn.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(() => SceneManager.LoadScene(_map.sceneToLoad_.name));
    }
}
