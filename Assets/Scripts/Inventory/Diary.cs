using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Diary : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemBase> items;



    private void Awake()
    {
        
    }

    public static Diary GetDiary()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Diary>();
    }

    public void AddItem(ItemBase item)
    {
        items.Add(item);
    }

    public List<ItemBase> GetItems()
    {
        return items;
    }

    public object CaptureState()
    {
        Debug.Log($"items:{items.Count}");
        List<string> names = new List<string>();
        for (int i = 0; i < items.Count; i++)
        {
            
            names.Add(items[i].Name);
        }
        return names;
    }

    public void RestoreState(object state)
    {
        items.Clear();
        for (int i = 0; i < ((List<string>)state).Count; i++)
        {
            
            items.Add(ItemsDB.GetObjectByName(((List<string>)state)[i]));
        }
        
    }

    public bool HasItem(ItemBase item)
    {
        return items.Exists(slot => slot == item);
    }
}
