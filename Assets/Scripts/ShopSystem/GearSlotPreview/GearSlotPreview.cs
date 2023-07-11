using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearSlotPreview : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentSO equipment;
    [SerializeField] private Image iconImg;
    [SerializeField] private Transform showPreviewItem;
    [SerializeField] private Animator anim;
    [SerializeField] private Sprite defaultDisplayImg;

    void Awake()
    {
        iconImg = transform.GetChild(1).GetComponent<Image>();
    }

    public void PreviewIconAndShow(EquipmentSO _item)
    {
        equipment = _item;
        iconImg.sprite = _item.itemIcon_;
        iconImg.enabled = true;

        showPreviewItem.GetComponent<Image>().sprite = _item.itemShow_;

        if (_item.itemSlots_ == ItemSlots.Wing)
        {
            string[] parameter = _item.itemID_.Split(new char[] { '_' });
            anim = showPreviewItem.GetComponent<Animator>();
            anim.SetInteger("Wing", int.Parse(parameter[1]));
            anim.enabled = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DisplayItemDescription();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ResetGearSlotPreview();
        }
    }

    public void DisplayItemDescription()
    {
        InventoryDesciptionController.instance.SetDescription(equipment);
    }

    //reset lại hình ảnh ở slot item và hiển thị hình ảnh xem trước
    public void ResetGearSlotPreview()
    {
        if(equipment != null)
        {
            iconImg.sprite = null;
            iconImg.enabled = false;

            showPreviewItem.GetComponent<Image>().sprite = defaultDisplayImg;
            if (equipment.itemSlots_ == ItemSlots.Wing)
            {
                anim.SetInteger("Wing", 0);
                anim.enabled = false;
            }
            equipment = null;
        }
    }
}
