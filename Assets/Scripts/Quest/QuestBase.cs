using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quest/Create a new quest")]
public class QuestBase : ScriptableObject
{
    [SerializeField] string objName;
    [SerializeField] string description;

    [SerializeField] Dialog startDialogue;
    [SerializeField] Dialog inProgressDialogue;
    [SerializeField] Dialog completedDialogue;

    [SerializeField] ItemBase requiredItem;
    [SerializeField] ItemBase rewardItem;

    public string Name => objName;
    public string Description => description;
    public Dialog StartDialogue => startDialogue;
    public Dialog InProgressDialogue => inProgressDialogue?.Lines?.Count > 0 ? inProgressDialogue : startDialogue;
    public Dialog CompletedDialogue => completedDialogue;

    public ItemBase RequiredItems => requiredItem;
    public ItemBase RewardItems => rewardItem;

}
