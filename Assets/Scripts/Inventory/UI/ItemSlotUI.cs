using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;

    public Text Name => nameText;

    public void SetData(ItemBase itemBase)
    {
        nameText.text = itemBase.Name;
    }
}
