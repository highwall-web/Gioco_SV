using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;

    [SerializeField] public int used = 0;

    public object CaptureState()
    {
        return used;
    }

    private void Start(){
        LoadAttribute();
        if (used == 1)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnDisable()
    {
        SaveAttribute();
    }

    private void LoadAttribute()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "used"))
        {
            used = PlayerPrefs.GetInt(gameObject.name + "used");
        }
    }

    private void SaveAttribute()
    {
        PlayerPrefs.SetInt(gameObject.name + "used", used);
        PlayerPrefs.Save();
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (used == 0)
        {
            initiator.GetComponent<Diary>().AddItem(item);

            used = 1;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            AudioManager.i.PlaySfx(AudioID.ItemObtained, pauseMusic: true);
            yield return DialogManager.Instance.ShowDialogText($"Hai trovato {item.Name}");
        }
    }

    public void RestoreState(object state)
    {
        used = (int)state;
        if(used == 1)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }
}
