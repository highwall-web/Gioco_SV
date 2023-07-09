using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    [SerializeField] QuestBase questToCheck;
    [SerializeField] ObjectActions onStart;
    [SerializeField] ObjectActions onComplete;

    [SerializeField] public string takeNpcName;

    [SerializeField] private bool quest = false;

    [SerializeField] private NPCController nPCController;

    QuestList questList;

    private void OnDestroy()
    {
        questList.OnUpdated -= UpdateObjectStatus;
    }

    private void Start()
    {
        questList = QuestList.GetQuestList();
        questList.OnUpdated += UpdateObjectStatus;
        if(GameObject.Find(takeNpcName) != null){
            nPCController = GameObject.Find(takeNpcName).GetComponent<NPCController>();
        }
        
    }

    private void Update(){
        if(!quest){
            quest = true;
            UpdateObjectStatus();
        }
        
    }

    public void UpdateObjectStatus()
    {
        if ((onStart != ObjectActions.DoNothing && questList.IsStarted(questToCheck.Name)) || nPCController.IsStarted == 1)
        {
            foreach (Transform child in transform)
            {
                if (onStart == ObjectActions.Enable)
                {
                    child.gameObject.SetActive(true);
                }
                else if(onStart == ObjectActions.Disable)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        if ((onComplete != ObjectActions.DoNothing && questList.IsCompleted(questToCheck.Name)) || nPCController.IsCompleted == 1)
        {
            foreach (Transform child in transform)
            {
                if (onComplete == ObjectActions.Enable)
                {
                    child.gameObject.SetActive(true);
                }
                else if (onComplete == ObjectActions.Disable)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}

public enum ObjectActions { DoNothing, Enable, Disable}
