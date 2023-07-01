using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private Button bagBtn;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform characterSystem;
    [SerializeField] private Transform upgradeSystem;
    [SerializeField] private Transform inventorySystem;
    [SerializeField] private Transform shopSystem;
    [SerializeField] private Transform itemDescription;
    [SerializeField] private Transform mapSystem;

    void Awake()
    {
        if (instance !=  null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }

    void Start()
    {
        characterSystem.gameObject.SetActive(false);
        upgradeSystem.gameObject.SetActive(false);
        inventorySystem.gameObject.SetActive(false);
        itemDescription.gameObject.SetActive(false);
        mapSystem.gameObject.SetActive(false);
    }
    
    public void OpenBag()
    {
        characterSystem.gameObject.SetActive(true);
        InventoryManager.instance.SetParentInventory(characterSystem);
        InventoryManager.instance.UpdateUIInventory();
    }

    public void CloseBag()
    {
        characterSystem.gameObject.SetActive(false);
        InventoryManager.instance.ResetParentInventory(canvas);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenUpgrade()
    {
        upgradeSystem.gameObject.SetActive(true);
        InventoryManager.instance.SetParentInventory(upgradeSystem);
        InventoryManager.instance.UpdateUIInventory();
    }

    public void CloseUpgrade()
    {
        UpgradeEquipmentManager.instance.ResetUppgradeSystem();
        CombineItemManager.instance.ResetCombineSystem();
        InventoryManager.instance.ResetParentInventory(canvas);
        upgradeSystem.gameObject.SetActive(false);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenShop()
    {
        shopSystem.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        shopSystem.gameObject.SetActive(false);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenMap()
    {
        mapSystem.gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        mapSystem.gameObject.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        InventoryManager.instance.OnApplicationQuit();
        UpgradeEquipmentManager.instance.OnApplicationQuit();
    }
}
