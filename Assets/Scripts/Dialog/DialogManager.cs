using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] ChoiceBox choiceBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnDialogFinished;

    public GameObject player;
    public Sprite dialogSpriteOld;
    public Sprite dialogSpriteNew;
    private SwapCharacter swapCharacter;
    private int actualCharacter;

    public static DialogManager Instance { get; private set; }

    private void Start()
    {
        swapCharacter = player.GetComponent<SwapCharacter>();
    }

    private void Awake()
    {
        Instance = this;
    }


    public bool IsShowing { get; private set; }

    internal IEnumerator ShowDialogText(string text, bool waitForInput = true, bool autoClose = true)
    {
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        AudioManager.i.PlaySfx(AudioID.UISelect);
        yield return TypeDialog(text);
        if(waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (autoClose)
        {
            CloseDialog();
        }
        OnDialogFinished?.Invoke();
    }

    public void CloseDialog()
    {
        
        dialogBox.SetActive(false);
        IsShowing = false;
    }

    public IEnumerator ShowDialog(Dialog dialog, List<string> choices = null,
        Action<int> onChoiceSelected = null)
    {
        yield return new WaitForEndOfFrame();

        actualCharacter = swapCharacter.getActualCharacter();
        if (actualCharacter == 0)
        {
            dialogBox.GetComponent<Image>().sprite = dialogSpriteOld;
            choiceBox.GetComponent<Image>().sprite = dialogSpriteOld;
        }
        else
        {
            dialogBox.GetComponent<Image>().sprite = dialogSpriteNew;
            choiceBox.GetComponent<Image>().sprite = dialogSpriteNew;
        }

        OnShowDialog?.Invoke();

        IsShowing = true;
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            AudioManager.i.PlaySfx(AudioID.UISelect);
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (choices != null && choices.Count > 1)
        {
            yield return choiceBox.ShowChoices(choices, onChoiceSelected, actualCharacter);
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        OnDialogFinished?.Invoke();
    }

    public void HandleUpdate()
    {
        
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
