using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDifficultyBtn : MonoBehaviour
{
    [SerializeField] private Sprite selectedImg;
    [SerializeField] private Sprite unselectedImg;

    public void Selected()
    {
        transform.GetComponent<Image>().sprite = selectedImg;
    }

    public void Unselected()
    {
        transform.GetComponent<Image>().sprite = unselectedImg;
    }
}
