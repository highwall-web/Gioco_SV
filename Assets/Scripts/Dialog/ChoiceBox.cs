using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] ChoiceText choiceTextPrefab;

    [SerializeField] Font fontAssetNew;
    [SerializeField] Font fontAssetOld;

    bool choiceSelected = false;

    List<ChoiceText> choiceTexts;
    int currentChoice;

    public IEnumerator ShowChoices(List<string> choices, Action<int> onChoiceSelected, int actualCharacter)
    {
        choiceSelected = false;
        currentChoice = 0;
        gameObject.SetActive(true);

        //Delete all existing choices
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        choiceTexts = new List<ChoiceText>();
        foreach (var choice in choices)
        {
            var choiceTextObj = Instantiate(choiceTextPrefab, transform);
            choiceTextObj.TextField.text = choice;
            if (actualCharacter == 0)
            {
                choiceTextObj.TextField.font = fontAssetOld;
                choiceTextObj.TextField.fontSize = 40;
            }
            else
            {
                choiceTextObj.TextField.font = fontAssetNew;
                choiceTextObj.TextField.fontSize = 30;
            }
            choiceTexts.Add(choiceTextObj);
        }
        yield return new WaitUntil(() => choiceSelected == true);

        onChoiceSelected?.Invoke(currentChoice);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentChoice;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentChoice;
        }

        currentChoice = Mathf.Clamp(currentChoice, 0, choiceTexts.Count - 1);

        for (int i = 0; i < choiceTexts.Count; i++)
        {
            choiceTexts[i].SetSelected(i == currentChoice);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            choiceSelected = true;
        }
    }
}
