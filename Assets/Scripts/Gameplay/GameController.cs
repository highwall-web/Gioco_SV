using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Paused}

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;

    GameState state;
    GameState stateBeforePause;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }


    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
            if (Input.GetKeyDown(KeyCode.S))
            {
                SavingSystem.i.Save("Saveslot_01");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SavingSystem.i.Load("Saveslot_01");
            }
        }
        else if( state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }
}
