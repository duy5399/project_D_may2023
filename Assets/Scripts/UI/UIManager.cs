using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Transform settingSystem;

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
        shopSystem.gameObject.SetActive(false);
        mapSystem.gameObject.SetActive(false);
        settingSystem.gameObject.SetActive(false);
    }
    
    public void OpenBag()
    {
        settingSystem.gameObject.SetActive(false);
        AudioManager.instance.ClickSuccessSFX();
        characterSystem.gameObject.SetActive(true);
        InventoryManager.instance.SetParentInventory(characterSystem);
        InventoryManager.instance.UpdateUIInventory();
    }

    public void CloseBag()
    {
        AudioManager.instance.ClickSuccessSFX();
        characterSystem.gameObject.SetActive(false);
        InventoryManager.instance.ResetParentInventory(canvas);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenUpgrade()
    {
        settingSystem.gameObject.SetActive(false);
        AudioManager.instance.ClickSuccessSFX();
        upgradeSystem.gameObject.SetActive(true);
        InventoryManager.instance.SetParentInventory(upgradeSystem);
        InventoryManager.instance.UpdateUIInventory();
    }

    public void CloseUpgrade()
    {
        AudioManager.instance.ClickSuccessSFX();
        UpgradeEquipmentManager.instance.ResetUppgradeSystem();
        CombineItemManager.instance.ResetCombineSystem();
        InventoryManager.instance.ResetParentInventory(canvas);
        upgradeSystem.gameObject.SetActive(false);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenShop()
    {
        settingSystem.gameObject.SetActive(false);
        AudioManager.instance.ClickSuccessSFX();
        ShopSystemManager.instance.LoadCurrency();
        shopSystem.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        AudioManager.instance.ClickSuccessSFX();
        ShopSystemManager.instance.RemoveEquipmentPreview();
        shopSystem.gameObject.SetActive(false);
        InventoryDesciptionController.instance.HiddenDescription();
    }

    public void OpenMap()
    {
        settingSystem.gameObject.SetActive(false);
        AudioManager.instance.ClickSuccessSFX();
        mapSystem.gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        AudioManager.instance.ClickSuccessSFX();
        mapSystem.gameObject.SetActive(false);
    }

    public void OpenSetting()
    {
        AudioManager.instance.ClickSuccessSFX();
        settingSystem.gameObject.SetActive(true);
    }

    public void CloseSetting()
    {
        AudioManager.instance.ClickSuccessSFX();
        settingSystem.gameObject.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        UpgradeEquipmentManager.instance.ResetUppgradeSystem();
        CombineItemManager.instance.ResetCombineSystem();
        CharacterEquipmentManager.instance.SaveCharacterEquipment();
        InventoryManager.instance.SaveDataInventory();
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    }

    public void ExitGame()
    {
        AudioManager.instance.ClickSuccessSFX();
        QuestionDialogUI.instance.DisplayConfirmQuitGame("Bạn có chắc muốn thoát game?", () => { Application.Quit(); }, () => { });
    }

    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called when the game is terminated
    void OnDisable()
    {
        //Debug.Log("OnDisable");
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
