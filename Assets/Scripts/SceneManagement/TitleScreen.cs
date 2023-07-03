using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject fader2;
    [SerializeField]public Fader fader;
    public float faderDuration = 0.5f;
    IEnumerator SwitchScene()
    {
        fader2.SetActive(true);
        yield return fader.FadeIn(faderDuration);
        yield return SceneManager.LoadSceneAsync("Gameplay");
        fader2.SetActive(false);
    }

    public void StartGame(){
        //Azioni da eseguire per iniziare il gioco
        //Carica la scena seguente
        StartCoroutine("SwitchScene");
    }

    public void QuitGame(){
        
            Application.Quit();
    }

    
}
