using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { FreeRoam, Dialog, Menu, Diary, Paused }

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] DiaryUI diaryUI;

    [SerializeField] Font fontAssetNew;
    [SerializeField] Font fontAssetOld;

    [SerializeField] Dialog dialogExit;

    private List<string> choicesExit;

    private SwapCharacter swapCharacter;
    public Sprite dialogSpriteOld;
    public Sprite dialogSpriteNew;

    GameState state;
    GameState prevState;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }



    MenuController menuController;
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();

        ItemsDB.Init();
        QuestDB.Init();
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void Start()
    {
        choicesExit = new List<string>() { "Si'", "No" };
        swapCharacter = playerController.GetComponent<SwapCharacter>();

        DialogManager.Instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnDialogFinished += () =>
        {
            if (state == GameState.Dialog)
            {
                state = prevState;
            }

        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs eliminati.");
        }

        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Diary)
        {
            Action onBack = () =>
            {
                diaryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            diaryUI.HandleUpdate(onBack);
        }

    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }

    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            // Diary
            if(swapCharacter.getActualCharacter() == 0)
            {
                diaryUI.GetComponent<Image>().sprite = dialogSpriteOld;
                Text[] textComponents = diaryUI.GetComponentsInChildren<Text>();
                foreach(Text textComponent in textComponents){
                    textComponent.font = fontAssetOld;
                    textComponent.fontSize = 32;
                }
            }
            else
            {
                diaryUI.GetComponent<Image>().sprite = dialogSpriteNew;
                Text[] textComponents = diaryUI.GetComponentsInChildren<Text>();
                foreach(Text textComponent in textComponents){
                    textComponent.font = fontAssetNew;
                    textComponent.fontSize = 25;
                }
            }
            diaryUI.gameObject.SetActive(true);
            state = GameState.Diary;
        }
        else if (selectedItem == 1)
        {
            // Save
            SavingSystem.i.Save("Saveslot_01");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 2)
        {
            // Load
            SavingSystem.i.Load("Saveslot_01");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3)
        {
            // Options
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 4)
        {
            // Quit
            StartCoroutine("ExitGame");
            state = GameState.FreeRoam;
        }
    }

    IEnumerator ExitGame()
    {
        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialog(dialogExit, choicesExit,
            (choiceIndex) => selectedChoice = choiceIndex);
        if (selectedChoice == 0)
        {
            StartCoroutine("SwitchScene");
        }
    }
    IEnumerator SwitchScene()
    {
        Destroy(gameObject.transform.parent.gameObject); // Destroys the DontDestroyOnLoad
        yield return SceneManager.LoadSceneAsync("TitleScreen");
    }
}
