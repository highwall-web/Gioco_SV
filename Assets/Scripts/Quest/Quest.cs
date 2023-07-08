using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestBase Base { get; private set; }
    public QuestStatus Status { get; private set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Base.Name,
            status = Status
        };
        return saveData;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
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

            AudioManager.i.PlaySfx(AudioID.ItemObtained, pauseMusic: true);

            yield return DialogManager.Instance.ShowDialogText($"Hai ricevuto {Base.RewardItems.Name}!");
        }

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
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

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}

public enum QuestStatus { None, Started, Completed}