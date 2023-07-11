using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ItemSO;

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
        bossName.text = _map.mapBoss_[_map.mapBoss_.Count - 1].bossPrefab_.name;
        bossImage.sprite = _map.mapBoss_[_map.mapBoss_.Count - 1].bossPrefab_.GetComponent<SpriteRenderer>().sprite;
        for(int i = 0; i < mapReward.Count; i++)
        {
            if (mapReward[i] != null)
            {
                mapReward[i].GetChild(1).GetComponent<Image>().sprite = _map.mapRewards_[i].item_.itemIcon_;
                mapReward[i].GetChild(2).GetComponent<TextMeshProUGUI>().text = _map.mapRewards_[i].quantity_.ToString();

                switch (_map.mapDifficulty_)
                {
                    case MapDifficulty.easy:
                        mapReward[i].GetChild(0).GetComponent<Image>().color = new Color32(209, 213, 216, 255);
                        break;
                    case MapDifficulty.normal:
                        mapReward[i].GetChild(0).GetComponent<Image>().color = new Color32(65, 168, 95, 255);
                        break;
                    case MapDifficulty.difficult:
                        mapReward[i].GetChild(0).GetComponent<Image>().color = new Color32(44, 130, 201, 255);
                        break;
                    case MapDifficulty.hero:
                        mapReward[i].GetChild(0).GetComponent<Image>().color = new Color32(147, 101, 184, 255);
                        break;
                    default:
                        Debug.Log("Not found mapDifficulty_: " + _map.mapDifficulty_);
                        break;
                }
            }
        }
        startBtn.onClick.RemoveAllListeners();
        
        startBtn.onClick.AddListener(delegate { onClickStartBtn(_map); });
        //startBtn.onClick.AddListener(delegate { onClickStartBtn(_map); });
    }

    private void onClickStartBtn(MapSO _map)
    {
        //Debug.Log(_map.sceneToLoad_.ToString());
        InventoryManager.instance.SaveDataInventory();
        CharacterEquipmentManager.instance.SaveCharacterEquipment();
        SceneManager.LoadScene(_map.sceneToLoad_.ToString());
    }
}
