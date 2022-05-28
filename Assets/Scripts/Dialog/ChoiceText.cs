using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceText : MonoBehaviour
{
    Text text;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color textColor;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void SetSelected(bool selected)
    {
        text.color = (selected)? highlightedColor : textColor;
    }

    public Text TextField => text;
}
