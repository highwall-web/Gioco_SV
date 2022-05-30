using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemBase item;
    [SerializeField] Dialog dialog;

    bool used = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        player.GetComponent<Diary>().AddItem(item);

        used = true;

        AudioManager.i.PlaySfx(AudioID.ItemObtained, pauseMusic: true);

        yield return DialogManager.Instance.ShowDialogText($"Hai ricevuto {item.Name}");
    }

    public bool CanBeGiven()
    {
        return item != null && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
