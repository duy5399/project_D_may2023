using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionDialogUI : MonoBehaviour
{
    public static QuestionDialogUI instance { get; private set; }
    [SerializeField] private TextMeshProUGUI questionTxt;
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;

    [SerializeField] private UnityAction yesAction;
    [SerializeField] private UnityAction noAction;

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
        LoadComponents();
        gameObject.SetActive(false);
    }

    public void LoadComponents()
    {
        questionTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        yesBtn = transform.GetChild(2).GetComponent<Button>();
        noBtn = transform.GetChild(3).GetComponent<Button>();
        closeBtn = transform.GetChild(4).GetComponent<Button>();
        itemIcon = transform.GetChild(5).GetComponent<Image>();
        itemName = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
    }

    public void ShowQuestion(string _question, Sprite _itemIcon, string _itemName, int _itemQuantity, int _itemPrice, UnityAction _yesAction, UnityAction _noAction)
    {
        questionTxt.text = _question;
        itemIcon.sprite = _itemIcon;
        itemName.text = _itemName + " x" + _itemQuantity;
        itemPrice.text = _itemPrice.ToString();

        yesAction = _yesAction;
        noAction = _noAction;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        yesBtn.onClick.AddListener(YesBtnClicked) ;
        noBtn.onClick.AddListener(NoBtnClicked);
        closeBtn.onClick.AddListener(HideQuestionDialogUI);
    }

    private void OnDisable()
    {
        if(yesBtn != null)
        {
            yesBtn.onClick.RemoveListener(YesBtnClicked);
        }
        if(noBtn != null)
        {
            noBtn.onClick.RemoveListener(NoBtnClicked);
        }
        closeBtn.onClick.RemoveListener(HideQuestionDialogUI);
    }

    private void YesBtnClicked()
    {
        HideQuestionDialogUI();
        //Like yesAction() but with a null check
        yesAction?.Invoke();
    }

    private void NoBtnClicked()
    {
        HideQuestionDialogUI();
        noAction?.Invoke();
    }

    public void HideQuestionDialogUI()
    {
        this.gameObject.SetActive(false);
    }
}
