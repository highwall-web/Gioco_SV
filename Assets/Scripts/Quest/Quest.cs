using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestBase Base { get; private set; }
    public QuestStatus Status { get; private set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);
    }

    public IEnumerator CompleteQuest()
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompletedDialogue);

        var diary = Diary.GetDiary();
        if( Base.RequiredItems != null)
        {
            //boh
        }

        if (Base.RewardItems != null)
        {
            diary.AddItem(Base.RewardItems);

            yield return DialogManager.Instance.ShowDialogText($"Hai ricevuto {Base.RewardItems.Name}!");
        }

        
    }
    public bool CanBeCompleted()
    {
        var diary = Diary.GetDiary();
        if (Base.RequiredItems != null)
        {
            if(!diary.HasItem(Base.RequiredItems)){
                return false;
            }
        }
        return true;
    }
}

public enum QuestStatus { None, Started, Completed}