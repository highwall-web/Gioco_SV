using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Menu, Diary, Paused}

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] DiaryUI diaryUI;

    GameState state;
    GameState stateBeforePause;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();

        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if( state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
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
            stateBeforePause = state;
            state = GameState.Paused;
        }
        else
        {
            state = stateBeforePause;
        }
    }

    void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if(Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if(state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if(state == GameState.Diary)
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
        if(selectedItem == 0)
        {
            // Diary
            diaryUI.gameObject.SetActive(true);
            state = GameState.Diary;
        }
        else if(selectedItem == 1)
        {
            // Save
            SavingSystem.i.Save("Saveslot_01");
            state = GameState.FreeRoam;
        }
        else if(selectedItem == 2)
        {
            // Load
            SavingSystem.i.Load("Saveslot_01");
            state = GameState.FreeRoam;
        }
        else if(selectedItem == 3)
        {
            // Options
            state = GameState.FreeRoam;
        }
        else if(selectedItem == 4)
        {
            // Quit
            state = GameState.FreeRoam;
        }
    }
}
