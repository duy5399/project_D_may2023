using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemSO;

public class EquippedSlotController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Animator anim;
    [SerializeField] private EquipmentSO itemEquipped;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemBorder;
    [SerializeField] private Text quantity;
    [SerializeField] private Transform playerDisplayImg;
    [SerializeField] private Sprite defaultDisplayImg;
    [SerializeField] private bool slotInUse;

    public EquipmentSO itemEquipped_ => itemEquipped;

    void Awake()
    {
        itemBorder = transform.GetChild(1).GetComponent<Image>();
        itemBorder.enabled = false;
        itemIcon = transform.GetChild(2).GetComponent<Image>();
        itemIcon.enabled = false;
        slotInUse = false;
        if (playerDisplayImg != null)
        {
            anim = playerDisplayImg.gameObject.GetComponent<Animator>();
        }
    }

    //equip gear for character
    public void EquipGear(EquipmentSO _item)
    {
        //if this slot is equipped, unequip and sent it to inventory before equip new gear
        if (slotInUse)
        {
            UnequipGear();
        }
        itemEquipped = _item;
        //update border cho slot
        switch (itemEquipped.itemTier_)
        {
            case RarityTier.common:
                itemBorder.color = new Color32(209, 213, 216, 255);
                break;
            case RarityTier.uncommmon:
                itemBorder.color = new Color32(65, 168, 95, 255);
                break;
            case RarityTier.rare:
                itemBorder.color = new Color32(44, 130, 201, 255);
                break;
            case RarityTier.epic:
                itemBorder.color = new Color32(147, 101, 184, 255);
                break;
            case RarityTier.legendary:
                itemBorder.color = new Color32(250, 197, 28, 255);
                break;
            case RarityTier.mythic:
                itemBorder.color = new Color32(226, 80, 65, 255);
                break;
            default:
                //Debug.Log("Not found rarity tier of item: " + equipment.itemName_);
                break;
        }
        itemBorder.enabled = true;
        //update img to equipped slot
        itemIcon.sprite = itemEquipped.itemIcon_;
        itemIcon.enabled = true;

        //push new gear to equipment list
        slotInUse = true;

        //update the display image
        if (itemEquipped != null && slotInUse)
        {
            if (itemEquipped.itemSlots_ != ItemSlots.Armlet && itemEquipped.itemSlots_ != ItemSlots.Ring && itemEquipped.itemSlots_ != ItemSlots.Offhand)
            {
                if (itemEquipped.itemSlots_ == ItemSlots.Weapon || itemEquipped.itemSlots_ == ItemSlots.Wing)
                {
                    DisplayAnimation();
                }
                else
                {
                    playerDisplayImg.gameObject.GetComponent<Image>().sprite = itemEquipped.itemShow_;
                }
            }
        }

        UpdatePlayerStats();
        DisplayPlayerStats();
        //Debug.Log("EquipGear successful => UpdateUIInventory ");
        InventoryManager.instance.UpdateUIInventory();
    }

    //display aniamtion of item (Wing, Face, ...)
    public void DisplayAnimation()
    {
        if (slotInUse)
        {
            if(itemEquipped.itemSlots_ == ItemSlots.Weapon)
            {
                //Debug.Log("item.itemStrength: " + itemEquipped.itemStrength_);
                anim.SetInteger("WeaponStrength", itemEquipped.itemStrength_);
                anim.enabled = true;
            }
            else if(itemEquipped.itemSlots_ == ItemSlots.Wing)
            {
                //Debug.Log("item.itemID wing: " + itemEquipped.itemID_);
                string[] parameter = itemEquipped.itemID_.Split(new char[] { '_' });
                anim.SetInteger("Wing", int.Parse(parameter[1]));
                anim.enabled = true;
            }
        }
        else
        {
            if (transform.name == "WeaponSlot")
            {
                //Debug.Log("reset item.itemStrength 11111111111111111111: " + 0);
                anim.SetInteger("WeaponStrength", 1);
                anim.enabled = false;
                //Debug.Log("reset item.itemStrength 22222222222222222222: " + 0);
            }
            else if (transform.name == "WingSlot")
            {
                anim.SetInteger("Wing", 0);
                anim.enabled = false;
            }
        }
    }

    //update stats for player after equip gear
    public void UpdatePlayerStats()
    {
        if(slotInUse)
        {
            for (int i = 0; i < itemEquipped.itemStats_.Length; i++)
            {
                switch (itemEquipped.itemStats_[i].attributes_.ToString())
                {
                    case "attack":
                        //Debug.Log("+++PlayerStats.instance.playerStat_.attack = " + PlayerStats.instance.playerStat_.attack_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetAttack(PlayerStats.instance.playerStat_.attack_ + itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "defense":
                        //Debug.Log("+++PlayerStats.instance.playerStat_.defense_ = " + PlayerStats.instance.playerStat_.defense_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetDefense(PlayerStats.instance.playerStat_.defense_ + itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "hp":
                        //Debug.Log("+++PlayerStats.instance.playerStat_.hitPoint_ = " + PlayerStats.instance.playerStat_.hitPoint_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetHitPoint(PlayerStats.instance.playerStat_.hitPoint_ + itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "luck":
                        //Debug.Log("+++PlayerStats.instance.playerStat_.luck_ = " + PlayerStats.instance.playerStat_.luck_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetLuck(PlayerStats.instance.playerStat_.luck_ + itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < itemEquipped.itemStats_.Length; i++)
            {
                switch (itemEquipped.itemStats_[i].attributes_.ToString())
                {
                    case "attack":
                        Debug.Log("---PlayerStats.instance.playerStat_.attack = " + PlayerStats.instance.playerStat_.attack_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetAttack(PlayerStats.instance.playerStat_.attack_ - (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_));
                        break;
                    case "defense":
                        Debug.Log("---PlayerStats.instance.playerStat_.defense_ = " + PlayerStats.instance.playerStat_.defense_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetDefense(PlayerStats.instance.playerStat_.defense_ - (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_));
                        break;
                    case "hp":
                        Debug.Log("---PlayerStats.instance.playerStat_.hitPoint_ = " + PlayerStats.instance.playerStat_.hitPoint_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetHitPoint(PlayerStats.instance.playerStat_.hitPoint_ - (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_));
                        break;
                    case "luck":
                        Debug.Log("---PlayerStats.instance.playerStat_.luck_ = " + PlayerStats.instance.playerStat_.luck_ + " itemEquipped.itemStats_[i].valueBasic_ = " + itemEquipped.itemStats_[i].valueBasic_);
                        PlayerStats.instance.SetLuck(PlayerStats.instance.playerStat_.luck_ - (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void DisplayPlayerStats()
    {
        CharacterEquipmentManager.instance.DisplayPlayerStats();
    }

    //unequip gear for character
    public void UnequipGear()
    {
        itemBorder.enabled = false;
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        slotInUse = false;
        UpdatePlayerStats();
        DisplayPlayerStats();

        InventoryManager.instance.AddItem(itemEquipped, 1);
        CharacterEquipmentManager.instance.RemoveItem(itemEquipped, 1);

        if (itemEquipped.itemSlots_ != ItemSlots.Armlet && itemEquipped.itemSlots_ != ItemSlots.Ring && itemEquipped.itemSlots_ != ItemSlots.Offhand) 
        {
            if (itemEquipped.itemSlots_ == ItemSlots.Weapon || itemEquipped.itemSlots_ == ItemSlots.Wing)
            {
                DisplayAnimation();
            }
            playerDisplayImg.gameObject.GetComponent<Image>().sprite = defaultDisplayImg;
        }
        itemEquipped = null;
        InventoryManager.instance.UpdateUIInventory();
    }

    //display description of item
    public void DisplayItemDescription()
    {
        InventoryDesciptionController.instance.SetDescription(itemEquipped);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (slotInUse)
        {
            //Debug.Log("EquippedSlot clickkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
            DisplayItemDescription();
            AudioManager.instance.ClickSuccessSFX();
        }
    }

    public void OnRightClick()
    {
        if(slotInUse)
        {
            UnequipGear();
            AudioManager.instance.UnequipGearSFX();
        }
        //Debug.Log("EquippedSlot clickzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
    }

    //void FixedUpdate()
    //{
    //    if (itemEquipped != null && slotInUse)
    //    {
    //        if (itemEquipped.itemSlots_ != ItemSlots.Armlet && itemEquipped.itemSlots_ != ItemSlots.Ring)
    //        {
    //            if (itemEquipped.itemSlots_ == ItemSlots.Weapon || itemEquipped.itemSlots_ == ItemSlots.Wing)
    //            {
    //                DisplayAnimation();
    //            }
    //            else
    //            {
    //                playerDisplayImg.gameObject.GetComponent<Image>().sprite = itemEquipped.itemShow_;
    //            }
    //        }
    //    }
    //}
}
