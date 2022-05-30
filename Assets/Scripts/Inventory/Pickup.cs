using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;
    public bool Used { get; set; } = false;

    public object CaptureState()
    {
        return Used;
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            initiator.GetComponent<Diary>().AddItem(item);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            AudioManager.i.PlaySfx(AudioID.ItemObtained, pauseMusic: true);
            yield return DialogManager.Instance.ShowDialogText($"Hai trovato {item.Name}");
        }
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if(Used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
