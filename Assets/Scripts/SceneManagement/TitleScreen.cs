using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    public void StartGame(){
        //Azioni da eseguire per iniziare il gioco
        //Carica la scena seguente
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame(){
        
            Application.Quit();
    }

    
}
