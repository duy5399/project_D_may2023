using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlotController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Animator anim;
    [SerializeField] private EquipmentSO itemEquipped;
    [SerializeField] private Image iconImg;
    [SerializeField] private Text quantity;
    [SerializeField] private Transform playerDisplayImg;
    [SerializeField] private Sprite defaultDisplayImg;
    [SerializeField] private bool slotInUse;

    public EquipmentSO itemEquipped_ => itemEquipped;

    void Awake()
    {
        iconImg = transform.GetChild(1).GetComponent<Image>();
        iconImg.enabled = false;
        if(playerDisplayImg != null)
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

        //update img to equipped slot
        iconImg.sprite = itemEquipped.itemIcon_;
        iconImg.enabled = true;

        //push new gear to equipment list
        slotInUse = true;

        //update the display image
        if (itemEquipped != null && slotInUse)
        {
            if (itemEquipped.itemSlots_ != ItemSlots.Armlet && itemEquipped.itemSlots_ != ItemSlots.Ring)
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
        ShowPlayerStats();
        Debug.Log("EquipGear successful => UpdateUIInventory ");
        InventoryManager.instance.UpdateUIInventory();
    }

    //display aniamtion of item (Wing, Face, ...)
    public void DisplayAnimation()
    {
        if (slotInUse)
        {
            if(itemEquipped.itemSlots_ == ItemSlots.Weapon)
            {
                Debug.Log("item.itemStrength: " + itemEquipped.itemStrength_);
                anim.SetInteger("WeaponStrength", itemEquipped.itemStrength_);
                anim.enabled = true;
            }
            else if(itemEquipped.itemSlots_ == ItemSlots.Wing)
            {
                Debug.Log("item.itemID wing: " + itemEquipped.itemID_);
                string[] parameter = itemEquipped.itemID_.Split(new char[] { '_' });
                anim.SetInteger("Wing", int.Parse(parameter[1]));
                anim.enabled = true;
            }
        }
        else
        {
            if (transform.name == "WeaponSlot")
            {
                Debug.Log("reset item.itemStrength 11111111111111111111: " + 0);
                anim.SetInteger("WeaponStrength", 1);
                anim.enabled = false;
                Debug.Log("reset item.itemStrength 22222222222222222222: " + 0);
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
                        PlayerStats.instance.attack += (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "defense":
                        PlayerStats.instance.defense += (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "hp":
                        PlayerStats.instance.hitPoint += (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "luck":
                        PlayerStats.instance.luck += (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
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
                        PlayerStats.instance.attack -= (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "defense":
                        PlayerStats.instance.defense -= (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "hp":
                        PlayerStats.instance.hitPoint -= (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    case "luck":
                        PlayerStats.instance.luck -= (itemEquipped.itemStats_[i].valueBasic_ + itemEquipped.itemStats_[i].valueBonusStrength_);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ShowPlayerStats()
    {
        PlayerStats.instance.UpdatePlayerStats();
    }

    //unequip gear for character
    public void UnequipGear()
    {
        iconImg.sprite = null;
        iconImg.enabled = false;

        slotInUse = false;
        UpdatePlayerStats();
        ShowPlayerStats();

        InventoryManager.instance.AddItem(itemEquipped);
        CharacterEquipmentManager.instance.RemoveItem(itemEquipped);

        if (itemEquipped.itemSlots_ != ItemSlots.Armlet && itemEquipped.itemSlots_ != ItemSlots.Ring)
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
            Debug.Log("EquippedSlot clickkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
            DisplayItemDescription();
        }
    }

    public void OnRightClick()
    {
        if(slotInUse)
        {
            UnequipGear();
        }
        Debug.Log("EquippedSlot clickzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
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
