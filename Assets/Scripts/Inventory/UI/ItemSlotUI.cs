using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;

    [SerializeField] Font fontAssetNew;
    [SerializeField] Font fontAssetOld;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Text Name => nameText;

    public float Height => rectTransform.rect.height;

    public void SetData(ItemBase itemBase)
    {
        nameText.text = itemBase.Name; 
    }
}
