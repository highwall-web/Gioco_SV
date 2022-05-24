using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemBase item;
    [SerializeField] Dialog dialog;

    bool used = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        player.GetComponent<Diary>().AddItem(item);

        used = true;

        yield return DialogManager.Instance.ShowDialogText($"Hai ricevuto {item.Name}");
    }

    public bool CanBeGiven()
    {
        return item != null && !used;
    }
}
