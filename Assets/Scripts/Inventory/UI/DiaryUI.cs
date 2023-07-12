using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Font fontAssetNew;
    [SerializeField] Font fontAssetOld;

    [SerializeField] Sprite pageSpriteOld;
    [SerializeField] Sprite pageSpriteNew;

    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Color highlightedColor;
    [SerializeField] Color textColor;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemDescription;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    int selectedItem = 0;
    const int itemsInViewport = 8;

    List<ItemSlotUI> slotUIList;
    RectTransform itemListRect;

    Diary diary;

    private void Awake()
    {
        diary = Diary.GetDiary();
        itemListRect = itemList.GetComponent<RectTransform>();
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateItemList();
    }

    public void UpdateItemList()
    {
        int actualCharacter = player.GetComponent<SwapCharacter>().getActualCharacter();
        //clear all existing items
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        slotUIList = new List<ItemSlotUI>();
        List<ItemBase> items = diary.GetItems();
        if (items != null && items.Count > 0)
        {
            /*if (actualCharacter == 0)
            {
                GameObject.Find("Icon").GetComponent<Image>().sprite = pageSpriteOld;
                Debug.Log(GameObject.Find("Icon").GetComponent<Image>().sprite);
            }
            else
            {
                GameObject.Find("Icon").GetComponent<Image>().sprite = pageSpriteNew;
                Debug.Log(GameObject.Find("Icon").GetComponent<Image>().sprite);
            }*/
            // La lista degli oggetti è valida e contiene almeno un elemento
            itemList.SetActive(true);
            
            foreach (var itemSlot in diary.GetItems())
            {
                var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
                slotUIObj.SetData(itemSlot);
                if (actualCharacter == 0)
                {
                    slotUIObj.Name.font = fontAssetOld;
                    slotUIObj.Name.fontSize = 32;
                }
                else
                {
                    slotUIObj.Name.font = fontAssetNew;
                    slotUIObj.Name.fontSize = 25;
                }

                slotUIList.Add(slotUIObj);
            }
            UpdateItemSelection();
            
        }
        else
        {
            // La lista degli oggetti è vuota o nulla
            itemList.SetActive(false);
        }
        
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

        
        selectedItem = Mathf.Clamp(selectedItem, 0, diary.GetItems().Count - 1);

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
        if (diary.GetItems() != null && diary.GetItems().Count > 0)
        {
            itemIcon.gameObject.SetActive(true);
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

                var items = diary.GetItems();
                if (selectedItem >= 0 && selectedItem < items.Count)
                {
                    var slot = items[selectedItem];
                    itemIcon.sprite = slot.Icon;
                    itemDescription.text = slot.Description;
                }

                HandleScrolling();
            }
        }
        else
        {
            // Gestisce il caso in cui la lista degli oggetti nel diario sia vuota o nulla
            itemIcon.gameObject.SetActive(false);
            itemDescription.text = "Ancora nessuna pagina nel diario.";
        }
    }

    void HandleScrolling()
    {
        bool hasItems = diary.GetItems().Count > 0;

        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport / 2, 0, selectedItem) * slotUIList[0].Height;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > itemsInViewport / 2 && hasItems;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + itemsInViewport / 2 < slotUIList.Count && hasItems;
        downArrow.gameObject.SetActive(showDownArrow);
    }
}
