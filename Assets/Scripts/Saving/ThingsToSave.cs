using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingsToSave : MonoBehaviour
{
    [SerializeField] public bool IsStarted = false;
    [SerializeField] public bool IsInProgress = false;
    [SerializeField] public bool IsCompleted = false;
    [SerializeField] public bool IsPickedUp = false;
    [SerializeField] public Quest activeQuest = null;
}
