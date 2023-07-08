using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;

    private GameObject essentialObjects;
    private ThingsToSave thingsToSave;
    public bool Used { get; set; } = false;

    public object CaptureState()
    {
        return Used;
    }

    private void Start(){
        essentialObjects = GameObject.Find("EssentialObjects");
        thingsToSave = essentialObjects.GetComponentInChildren<ThingsToSave>();

        if (thingsToSave.IsPickedUp){
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            initiator.GetComponent<Diary>().AddItem(item);

            Used = true;
            thingsToSave.IsPickedUp = Used;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            AudioManager.i.PlaySfx(AudioID.ItemObtained, pauseMusic: true);
            yield return DialogManager.Instance.ShowDialogText($"Hai trovato {item.Name}");
        }
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;
        thingsToSave.IsPickedUp = Used;
        if(Used)
        {
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }
}
