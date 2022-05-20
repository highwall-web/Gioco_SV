using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    [SerializeField] List<ItemBase> items;

    public List<ItemBase> Items => items;

    public static Diary GetDiary()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Diary>();
    }
}
