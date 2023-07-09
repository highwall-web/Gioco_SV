using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [Header("Dialog")]
    [SerializeField] Dialog dialog;
    [SerializeField] bool hasChoiceBox = false;
    [SerializeField] public List<string> choiceOptions;
    [SerializeField] public List<string> answerOptions;//gestisce le risposte del chiocebox

    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    [Header("Movement")]
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;
    
    private GameObject essentialObjects;
    private ThingsToSave thingsToSave;

    public bool hasQuest;

    [SerializeField] public int IsStarted;
    [SerializeField] public int IsInProgress;
    [SerializeField] public int IsCompleted;


    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;
    Quest activeQuest;

    Character character;
    ItemGiver itemGiver;

    private void Awake()
    {
        character = GetComponent<Character>();
        itemGiver = GetComponent<ItemGiver>();
    }

    private void Start(){
        if (hasQuest)
        {
            LoadAttribute();
        }
        
    }

    private void OnDisable()
    {
        if (hasQuest)
        {
            SaveAttribute();
        }
    }

    private void LoadAttribute()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "IsStarted"))
        {
            IsStarted = PlayerPrefs.GetInt(gameObject.name + "IsStarted");
        }
        if (PlayerPrefs.HasKey(gameObject.name + "IsInProgress"))
        {
            IsInProgress = PlayerPrefs.GetInt(gameObject.name + "IsInProgress");
        }
        if (PlayerPrefs.HasKey(gameObject.name + "IsCompleted"))
        {
            IsCompleted = PlayerPrefs.GetInt(gameObject.name + "IsCompleted");
        }
    }

    private void SaveAttribute()
    {
        PlayerPrefs.SetInt(gameObject.name + "IsStarted", IsStarted);
        PlayerPrefs.SetInt(gameObject.name + "IsInProgress", IsInProgress);
        PlayerPrefs.SetInt(gameObject.name + "IsCompleted", IsCompleted);
        PlayerPrefs.Save();
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.lookTowards(initiator.position);
            if (hasQuest)
            {
                if (questToComplete != null && IsCompleted == 0)
                {
                    var quest = new Quest(questToComplete);
                    yield return quest.CompleteQuest();
                    questToComplete = null;
                    IsCompleted = 1;

                    //Debug.Log($"{quest.Base.Name} completed!");
                }

                if (itemGiver != null && itemGiver.CanBeGiven())
                {
                    yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
                }
                else if (questToStart != null && IsStarted == 0 && IsInProgress == 0 && IsCompleted == 0)
                {
                    activeQuest = new Quest(questToStart);
                    yield return activeQuest.StartQuest();
                    IsStarted = 1;
                    IsInProgress = 1;

                    if (activeQuest.CanBeCompleted() && thingsToSave.IsInProgress)
                    {
                        yield return activeQuest.CompleteQuest();
                        activeQuest = null;
                        IsCompleted = 1;
                        IsInProgress = 0;
                        activeQuest = null;
                    }
                    questToStart = null;
                }
                else if (activeQuest != null && IsStarted == 1 && IsInProgress == 1 && IsCompleted == 0 )
                {

                    if (activeQuest.CanBeCompleted())
                    {
                        yield return activeQuest.CompleteQuest();
                        activeQuest = null;
                        IsCompleted = 1;
                        IsInProgress = 0;
                        activeQuest = null;
                    }
                    else
                    {
                        yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InProgressDialogue);
                    }
                }
                else
                {
                    //gestisce il dialog dopo choice
                    if (hasChoiceBox)
                    {
                        int selectedChoice = 0;
                        yield return DialogManager.Instance.ShowDialog(dialog, choiceOptions,
                            (choiceIndex) => selectedChoice = choiceIndex);
                        // Gestisce l'opzione selezionata
                        for( int i=0; i < choiceOptions.Count; i++){
                            if( selectedChoice == i){
                                yield return DialogManager.Instance.ShowDialogText(answerOptions[i]);
                            }
                        }
                    }
                    else
                    {
                        yield return DialogManager.Instance.ShowDialog(dialog);
                    }

                }
            }
            else
            {
                if (hasChoiceBox)
                {
                    int selectedChoice = 0;
                    yield return DialogManager.Instance.ShowDialog(dialog, choiceOptions,
                        (choiceIndex) => selectedChoice = choiceIndex);
                    // Gestisce l'opzione selezionata
                    for( int i=0; i < choiceOptions.Count; i++){
                        if( selectedChoice == i){
                            yield return DialogManager.Instance.ShowDialogText(answerOptions[i]);
                        }
                    }
                }
                else
                {
                    yield return DialogManager.Instance.ShowDialog(dialog);
                }
            }

            idleTimer = 0f;
            state = NPCState.Idle;
        }
            
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(walk());
            }
        }
        character.HandleUpdate();
    }

    IEnumerator walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;
        
        yield return character.Move(movementPattern[currentPattern]);
        if (transform.position != oldPos )
        {
            currentPattern = (currentPattern + 1) % movementPattern.Count;
        }
        

        state = NPCState.Idle;
    }

    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();
        saveData.activeQuest = activeQuest?.GetSaveData();

        if(questToStart != null)
            saveData.questToStart = (new Quest(questToStart)).GetSaveData();

        if(questToComplete != null)
            saveData.questToComplete = (new Quest(questToComplete)).GetSaveData();

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;
        if(saveData != null)
        {
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;

            questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;

            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;
        }
    }
}

[System.Serializable]
public class NPCQuestSaveData
{
    public QuestSaveData activeQuest;
    public QuestSaveData questToStart;
    public QuestSaveData questToComplete;
}

public enum NPCState { Idle, Walking, Dialog }