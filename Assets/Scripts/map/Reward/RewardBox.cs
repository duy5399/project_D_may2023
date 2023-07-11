using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static ItemSO;

public class RewardBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<MapRewards> mapRewards;
    [SerializeField] private bool beenClicked;

    [Header("Display Item Drop From BOSS")]
    [SerializeField] private Transform rewardList;
    [SerializeField] private GameObject rewardItem;
    private Dictionary<string, GameObject> rewardSlotDisplayed = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        mapRewards = MapInfo.instance.mapInfo_.mapRewards_;
        rewardList = transform.GetChild(0).GetChild(0);
        rewardList.gameObject.SetActive(false);

        string resourcePath = "Prefabs/map/RewardBox/RewardItem";
        rewardItem = Resources.Load<GameObject>(resourcePath);

        beenClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("Mouse Click Detected");
        if (!beenClicked)
        {
            transform.GetComponent<Animator>().SetTrigger("OpenBox");
            rewardList.gameObject.SetActive(true);
            RandomRewardToList();
            beenClicked = true;
        }
    }

    public void OnClickOpen()
    {
        //Debug.Log("Mouse Click Detected");
        //if (!beenClicked)
        //{
        //    transform.GetComponent<Animator>().SetTrigger("OpenBox");
        //    rewardList.gameObject.SetActive(true);
        //    RandomRewardToList();
        //    beenClicked = true;
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!beenClicked)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                rewardList.gameObject.SetActive(true);
                RandomRewardToList();
            }
            beenClicked = true;
        }
    }

    //hiển thị danh sách vật phẩm rơi ra sau khi chiến thắng (đánh bại BOSS)
    private void RandomRewardToList()
    {
        int numberOfItemsDropped = Random.Range(5, 30);
        Transform parentRewardList = rewardList.GetChild(0).GetChild(0);
        //Debug.Log("numberOfItemsDropped = " + numberOfItemsDropped);
        for (int i = 0; i < numberOfItemsDropped; i++)
        {
            MapRewards reward = GetRandomItemFromMapRewards(mapRewards);
            if (reward != null)
            {
                if (reward.item_.itemType_ == ItemSO.ItemType.Equipment)
                {
                    EquipmentSO equipment = ScriptableObject.Instantiate((EquipmentSO)reward.item_);

                    switch (MapInfo.instance.mapInfo_.mapDifficulty_)
                    {
                        case MapDifficulty.easy:
                            equipment.RandomTier(0, 2);
                            break;
                        case MapDifficulty.normal:
                            equipment.RandomTier(1, 3);
                            break;
                        case MapDifficulty.difficult:
                            equipment.RandomTier(2, 4);
                            break;
                        case MapDifficulty.hero:
                            equipment.RandomTier(3, 4);
                            break;
                    }
                    equipment.RandomStats(equipment.itemTier_);
                    equipment.SetItemID(equipment.itemID_ + "_" + equipment.itemTier_ + "_" + equipment.GetStatsInfo() + "_" + equipment.GetRandomItemID());
                    GameObject obj = Instantiate(rewardItem, parentRewardList);
                    obj.GetComponent<RewardItem>().AddItemInfoToSlot(equipment, reward.quantity_);
                    InventoryManager.instance.AddItem(equipment, reward.quantity_);
                    //Debug.Log("Vật phẩm thứ [ " + i + "]: " + equipment.itemName_ + " - " + reward.quantity_);
                }
                else if (reward.item_.itemType_ == ItemSO.ItemType.Material)
                {
                    MaterialSO material = ScriptableObject.Instantiate((MaterialSO)reward.item_);

                    if (rewardSlotDisplayed.ContainsKey(material.itemID_))
                    {
                        rewardSlotDisplayed[material.itemID_].GetComponent<RewardItem>().SetQuantity(rewardSlotDisplayed[material.itemID_].GetComponent<RewardItem>().quantity_ + reward.quantity_);
                        rewardSlotDisplayed[material.itemID_].GetComponentInChildren<TextMeshProUGUI>().text = rewardSlotDisplayed[material.itemID_].GetComponent<RewardItem>().quantity_.ToString();
                    }
                    else
                    {
                        GameObject obj = Instantiate(rewardItem, parentRewardList);
                        obj.GetComponent<RewardItem>().AddItemInfoToSlot(material, reward.quantity_);
                        rewardSlotDisplayed.Add(material.itemID_, obj);
                    }
                    InventoryManager.instance.AddItem(material, reward.quantity_);
                    //Debug.Log("Vật phẩm thứ [ " + i + "]: " + material.itemName_ + " - " + reward.quantity_);
                }
            }
            else
            {

            }
                //Debug.Log("Lỗi khi tạo vật phẩm ngẫu nhiên");
        }
        InventoryManager.instance.SaveDataInventory();
    }

    //lấy ngẫu nhiên (có tỉ lệ) vật phẩm có thể rớt trong phó bản đang khiêu chiến
    private MapRewards GetRandomItemFromMapRewards(List<MapRewards> _mapRewards)
    {
        float random = Random.Range(0f, 1f);
        //Debug.Log("tỉ lệ vật phẩm => " + random);
        float cumulativeChance = 0;
        for (int i = 0; i <  _mapRewards.Count; i++)
        {
            cumulativeChance += (_mapRewards[i].dropRate_/100);
            if (random < cumulativeChance)
            {
                return _mapRewards[i];
            }
        }
        return null;
    }

    public void DisplayItemDescription()
    {
        //InventoryDesciptionController.instance.SetDescription(equipment);
    }

    public void onClickConfirmBtn()
    {
        rewardList.gameObject.SetActive(false);
        StartCoroutine(AutoReturnHome(3f));
    }

    IEnumerator AutoReturnHome(float _interval)
    {
        UIController.instance.OnAlertWarning("Tự động về lại trang chủ sau " + _interval + " giây!!!");
        yield return new WaitForSeconds(_interval);
        MapInfo.instance.Destroy();
        AudioManager.instance.Destroy();
        SceneManager.LoadScene("Homepage");
    }
}
