using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string objName;
    [TextArea(minLines:1,maxLines:64)]
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    //[SerializeField] public bool already_used = false;

    public string Name => name;
    public string Description => description;
    public Sprite Icon => icon;
}
