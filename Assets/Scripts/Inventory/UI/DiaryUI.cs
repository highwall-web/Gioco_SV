using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Color highlightedColor;
    [SerializeField] Color textColor;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemDescription;

    int selectedItem = 0;

    List<ItemSlotUI> slotUIList;

    Diary diary;

    private void Awake()
    {
        diary = Diary.GetDiary();
    }

    private void Start()
    {
        UpdateItemList();
    }

    void UpdateItemList()
    {
        //clear all existing items
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in diary.Items)
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }

        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack)
    {
        int prevSelection = selectedItem;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --selectedItem;
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, diary.Items.Count - 1);

        if (prevSelection != selectedItem)
        {
            UpdateItemSelection();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
            {
                slotUIList[i].Name.color = highlightedColor;
            }
            else
            {
                slotUIList[i].Name.color = textColor;
            }

            var slot = diary.Items[selectedItem];
            itemIcon.sprite = slot.Icon;
            itemDescription.text = slot.Description;
        }
    }
}
