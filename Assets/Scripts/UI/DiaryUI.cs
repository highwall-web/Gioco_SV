using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryUI : MonoBehaviour
{
    public void HandleUpdate(Action onBack)
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("ONBACK INVOKE");
            onBack?.Invoke();
        }
    }
}
