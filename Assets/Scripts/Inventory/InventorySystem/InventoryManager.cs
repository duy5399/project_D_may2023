using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : BagManager
{
    #region Singleton
    public static InventoryManager instance { get; private set; }
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
    #endregion

    #region Inventory

    [SerializeField] private int space = 200;
    [SerializeField] private string savePathEquipmentInventory = "/inventoryEquipment.lsd";
    [SerializeField] private string savePathMaterialInventory = "/inventoryMaterial.lsd";

    [SerializeField] private Transform equipmentParent; //Canvas - InventorySystem - Equipments - Viewport - InventoryEquipment
    [SerializeField] private Transform materialParent; //Canvas - InventorySystem - Marterials - Viewport - InventoryMaterial

    [SerializeField] private GameObject inventorySlotEquipment; //Resources - Prefab - Inventory - InventorySlotEquipment
    [SerializeField] private GameObject inventorySlotMaterial; //Resources - Prefab - Inventory - InventorySlotMaterial

    private Dictionary<InventoryEquipmentSlot, GameObject> inventoryEquipmentSlotDisplayed = new Dictionary<InventoryEquipmentSlot, GameObject>();
    private Dictionary<InventoryMaterialSlot, GameObject> inventoryMaterialSlotDisplayed = new Dictionary<InventoryMaterialSlot, GameObject>();

    //thêm trang bị hoặc tài nguyên vào kho đồ
    public override bool AddItem(ItemSO _item)
    {
        if (inventorySO.listEquipmentSO_.equipments.Count + inventorySO.listMaterialSO_.materials.Count  > space)
        {
            Debug.Log("Not enough space");
            return false;
        }
        inventorySO.AddItem(_item, 1);
        return true;
    }

    //xóa trang bị hoặc tài nguyên ra khỏi kho đồ
    public override bool RemoveItem(ItemSO _item)
    {

        inventorySO.RemoveItem(_item, 1);
        return true;
    }

    //set parent cho inventory (mở trang bị nhân vật thì củn hiển thị kho đồ và set parent là CharacterSystem)
    public void SetParentInventory(Transform inventoryParent)
    {
        transform.SetParent(inventoryParent);
        transform.gameObject.SetActive(true);
    }

    //reset parent (set parent = canvas)
    public void ResetParentInventory(Transform inventoryParent)
    {
        transform.SetParent(inventoryParent);
        transform.gameObject.SetActive(false);
    }

    //cập nhật lại hình ảnh, số lượng vật phẩm hiển thị trong kho đồ
    public void UpdateUIInventory()
    {
        for(int i = 0; i < inventorySO.listEquipmentSO_.equipments.Count; i++)
        {
            //dùng Dictionary để kiểm tra các vật phẩm đã hiển thị, nếu có key tồn tại (hiển thị rồi) thì chỉ cần cập nhật lại số lượng
            if (inventoryEquipmentSlotDisplayed.ContainsKey(inventorySO.listEquipmentSO_.equipments[i]))
            {
                inventoryEquipmentSlotDisplayed[inventorySO.listEquipmentSO_.equipments[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventorySO.listEquipmentSO_.equipments[i].quantity_.ToString();
            }
            else
            {
                GameObject obj = Instantiate(inventorySlotEquipment, equipmentParent);
                obj.GetComponent<InventorySlotEquipmentController>().AddItemInfo(inventorySO.listEquipmentSO_.equipments[i], inventorySO.listEquipmentSO_.equipments[i].quantity_);
                inventoryEquipmentSlotDisplayed.Add(inventorySO.listEquipmentSO_.equipments[i], obj);
            }
        }
        for (int i = 0; i < inventorySO.listMaterialSO_.materials.Count; i++)
        {
            if (inventoryMaterialSlotDisplayed.ContainsKey(inventorySO.listMaterialSO_.materials[i]))
            {
                inventoryMaterialSlotDisplayed[inventorySO.listMaterialSO_.materials[i]].GetComponent<InventorySlotMaterialController>().SetQuantity(inventorySO.listMaterialSO_.materials[i].quantity_);
                Debug.Log("inventoryMaterialSlotDisplayed.ContainsKey: " + i);
                inventoryMaterialSlotDisplayed[inventorySO.listMaterialSO_.materials[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventorySO.listMaterialSO_.materials[i].quantity_.ToString();
            }
            else
            {
                GameObject obj = Instantiate(inventorySlotMaterial, materialParent);
                obj.GetComponent<InventorySlotMaterialController>().AddItemInfo(inventorySO.listMaterialSO_.materials[i], inventorySO.listMaterialSO_.materials[i].quantity_);
                inventoryMaterialSlotDisplayed.Add(inventorySO.listMaterialSO_.materials[i], obj);
            }
        }
    }
    #endregion

    public void OnApplicationQuit()
    {
        //inventorySO.listItemSO_.items.Clear();
        inventorySO.listEquipmentSO_.equipments.Clear();
        inventorySO.listMaterialSO_.materials.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventorySO.SaveEquipment(savePathEquipmentInventory);
            inventorySO.SaveMaterial(savePathMaterialInventory);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventorySO.LoadEquipment(savePathEquipmentInventory);
            inventorySO.LoadMaterial(savePathMaterialInventory);
            UpdateUIInventory();
        }
    }
}
