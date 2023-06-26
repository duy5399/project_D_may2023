using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image iconImg;
    [SerializeField] protected TextMeshProUGUI quantityTxt;
    [SerializeField] protected ItemSO item;
    [SerializeField] protected int quantity = 0;

    public ItemSO item_ => item;
    public int quantity_ => quantity;

    protected virtual void Awake()
    {
        LoadComponents();
    }

    public virtual void LoadComponents()
    {
        iconImg = transform.GetChild(0).GetComponent<Image>();
        quantityTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    //add new item
    public virtual void AddItem(ItemSO _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
        iconImg.sprite = item.itemIcon_;
        quantityTxt.text = quantity.ToString();
    }

    //display description of item
    public virtual void DisplayItemDescription()
    {
        InventoryDesciptionController.instance.SetDescription(item);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick"); 
    }

    public void OnLeftClick()
    {
        DisplayItemDescription();
    }

    public virtual void OnRightClick()
    {
        Debug.Log("OnRightClick");
    }
}
