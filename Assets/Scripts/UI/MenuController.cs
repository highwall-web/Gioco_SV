using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color textColor;

    public GameObject player;
    private SwapCharacter swapCharacter;
    private int actualCharacter;
    public Sprite dialogSpriteOld;
    public Sprite dialogSpriteNew;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    List<Text> menuItems;

    int selectedItem = 0;

    private void Start()
    {
        swapCharacter = player.GetComponent<SwapCharacter>();
    }

    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }

    public bool IsShowing { get; private set; }

    public void OpenMenu()
    {
        actualCharacter = swapCharacter.getActualCharacter();

        if (actualCharacter == 0)
        {
            menu.GetComponent<Image>().sprite = dialogSpriteOld;
        }
        else
        {
            menu.GetComponent<Image>().sprite = dialogSpriteNew;
        }

        IsShowing = true;
        menu.SetActive(true);
        UpdateItemSelection();
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        IsShowing = false;
    }

    public void HandleUpdate()
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

        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if(prevSelection != selectedItem)
        {
            UpdateItemSelection();
        }

        if(Input.GetKey(KeyCode.Z))
        {
            onMenuSelected?.Invoke(selectedItem);
            CloseMenu();
        }

        else if (Input.GetKey(KeyCode.X))
        {
            onBack?.Invoke();
            CloseMenu();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if(i == selectedItem)
            {
                menuItems[i].color = highlightedColor;
            }
            else
            {
                menuItems[i].color = textColor;
            }
        }
    }
}
