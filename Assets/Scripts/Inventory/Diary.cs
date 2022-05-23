using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Diary : MonoBehaviour
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
}
